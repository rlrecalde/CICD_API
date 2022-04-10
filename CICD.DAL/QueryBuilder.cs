using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CICD.DAL
{
    /// <summary>
    /// Constructs a SQL query
    /// </summary>
    public class QueryBuilder
    {
        private string _query;

        public enum Operators
        {
            EqualTo, NotEqualTo, GreaterThan, GreaterThanOrEqualTo, LessThan, LessThanOrEqualTo, IsNull, IsNotNull, In, Like
        }

        public enum Orders
        {
            Ascending, Descending
        }

        private Dictionary<Operators, string> _operators;
        private Dictionary<Type, Func<object, string>> _valueTypes;
        private Dictionary<Type, string> _identityIds;
        private bool _isWhereClause;

        /// <summary>
        /// This property is used to determine if you have already put a WHERE clause on the query, when you are making a conditional predicate.
        /// </summary>
        public bool IsWhereClause { get => _isWhereClause; private set => _isWhereClause = value; }

        /// <summary>
        /// Fluent API Query Builder
        /// </summary>
        /// <param name="numbersCultureInfo">Culture Information for decimal numbers</param>
        /// <param name="dateTimeFormat">Date Format for conditional clauses</param>
        public QueryBuilder(CultureInfo numbersCultureInfo, string dateTimeFormat)
        {
            this._query = string.Empty;

            this._operators = new Dictionary<Operators, string>
            {
                { Operators.EqualTo, "=" },
                { Operators.NotEqualTo, "<>" },
                { Operators.GreaterThan, ">" },
                { Operators.GreaterThanOrEqualTo, ">=" },
                { Operators.LessThan, "<" },
                { Operators.LessThanOrEqualTo, "<=" },
                { Operators.IsNull, "IS NULL" },
                { Operators.IsNotNull, "IS NOT NULL" },
                { Operators.In, "IN"},
                { Operators.Like, "LIKE"},
            };

            this._valueTypes = new Dictionary<Type, Func<object, string>>
            {
                { typeof(byte), (value) => value.ToString() },
                { typeof(short), (value) => value.ToString() },
                { typeof(int), (value) => value.ToString() },
                { typeof(long), (value) => value.ToString() },
                { typeof(float), (value) => ((float)value).ToString(numbersCultureInfo) },
                { typeof(decimal), (value) => ((decimal)value).ToString(numbersCultureInfo) },
                { typeof(double), (value) => ((double)value).ToString(numbersCultureInfo) },
                { typeof(string), (value) => $"'{(string)value}'" },
                { typeof(bool), (value) => ((bool)value) ? "1" : "0" },
                { typeof(DateTime), (value) => $"'{((DateTime)value).ToString(dateTimeFormat)}'" },
            };

            this._identityIds = new Dictionary<Type, string>
            {
                { typeof(SqlConnection), " SELECT CAST(SCOPE_IDENTITY() AS BIGINT)" },
                { typeof(SqliteConnection), " SELECT CAST(last_insert_rowid() AS BIGINT)" },
            };

            this._isWhereClause = false;
        }

        /// <summary>
        /// Constructs the SELECT statement
        /// <para>Usage:</para>
        /// <para>.Select&lt;Entity&gt;(x => new { x.Prop1, x.Prop2, ... })</para>
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the "from" SELECT statement</typeparam>
        /// <param name="properties">Lambda Expression for the Entity Properties (Table Columns) for the SELECT statement</param>
        /// <returns>this</returns>
        public QueryBuilder Select<T>(Expression<Func<T, dynamic>> properties, int top = 0)
        {
            Type typeOfT = this.GetEntityType(properties.Parameters);
            var entity = Activator.CreateInstance(typeOfT);
            string table = this.GetEntityName(entity);

            string[] propertyNames = this.GetPropertyNamesFromDynamicExpression(entity, properties.Body);
            string fields = this.GetFields(table, propertyNames);

            this._query = "SELECT ";
            if (top > 0)
                this._query += $"TOP {top} ";

            this._query += $"{fields} FROM {table}";

            return this;
        }

        /// <summary>
        /// Constructs the INNER JOIN statement
        /// <para>Usage:</para>
        /// <para>.InnerJoin&lt;Entity1, Entity2&gt;(e1 => e1.Prop, e2 => e2.Prop, e2 => new { e2.Prop1, e2.Prop2, ... })</para>
        /// </summary>
        /// <typeparam name="T1">Type of the source Entity/Table to join to</typeparam>
        /// <typeparam name="T2">Type of the target Entity/Table to be joinned</typeparam>
        /// <param name="propertyEntity1">Lambda Expression for the source Entity Property (Table Column) for the left side of the ON clause</param>
        /// <param name="propertyEntity2">Lambda Expression for the target Entity Property (Table Column) for the right side of the ON clause</param>
        /// <param name="properties">Lambda Expression for the target Entity Properties (Table Columns) to be included on the SELECT statement</param>
        /// <returns>this</returns>
        public QueryBuilder InnerJoin<T1, T2>(Expression<Func<T1, object>> propertyEntity1, Expression<Func<T2, object>> propertyEntity2, Expression<Func<T2, dynamic>>? properties = null)
        {
            Type typeOfT1 = this.GetEntityType(propertyEntity1.Parameters);
            Type typeOfT2 = this.GetEntityType(propertyEntity2.Parameters);
            var entity1 = Activator.CreateInstance(typeOfT1);
            var entity2 = Activator.CreateInstance(typeOfT2);
            string table1 = this.GetEntityName(entity1);
            string table2 = this.GetEntityName(entity2);

            string property1Name = this.GetPropertyNameFromObjectExpression(entity1, propertyEntity1.Body);
            string property2Name = this.GetPropertyNameFromObjectExpression(entity2, propertyEntity2.Body);

            if (properties != null)
            {
                string[] propertyNames = this.GetPropertyNamesFromDynamicExpression(entity2, properties.Body);
                string fields = this.GetFields(table2, propertyNames);
                this.InsertNewPropertiesIntoQuery(fields);
            }

            this._query += $" INNER JOIN {table2} ON {table1}.{property1Name} = {table2}.{property2Name}";

            return this;
        }

        /// <summary>
        /// Constructs the LEFT JOIN statement
        /// <para>Usage:</para>
        /// <para>.LeftJoin&lt;Entity1, Entity2&gt;(e1 => e1.Prop, e2 => e2.Prop, e2 => new { e2.Prop1, e2.Prop2, ... })</para>
        /// </summary>
        /// <typeparam name="T1">Type of the source Entity/Table to join to</typeparam>
        /// <typeparam name="T2">Type of the target Entity/Table to be joinned</typeparam>
        /// <param name="propertyEntity1">Lambda Expression for the source Entity Property (Table Column) for the left side of the ON clause</param>
        /// <param name="propertyEntity2">Lambda Expression for the target Entity Property (Table Column) for the right side of the ON clause</param>
        /// <param name="properties">Lambda Expression for the target Entity Properties (Table Columns) to be included on the SELECT statement</param>
        /// <returns>this</returns>
        public QueryBuilder LeftJoin<T1, T2>(Expression<Func<T1, object>> propertyEntity1, Expression<Func<T2, object>> propertyEntity2, Expression<Func<T2, dynamic>>? properties = null)
        {
            Type typeOfT1 = this.GetEntityType(propertyEntity1.Parameters);
            Type typeOfT2 = this.GetEntityType(propertyEntity2.Parameters);
            var entity1 = Activator.CreateInstance(typeOfT1);
            var entity2 = Activator.CreateInstance(typeOfT2);
            string table1 = this.GetEntityName(entity1);
            string table2 = this.GetEntityName(entity2);

            string property1Name = this.GetPropertyNameFromObjectExpression(entity1, propertyEntity1.Body);
            string property2Name = this.GetPropertyNameFromObjectExpression(entity2, propertyEntity2.Body);

            if (properties != null)
            {
                string[] propertyNames = this.GetPropertyNamesFromDynamicExpression(entity2, properties.Body);
                string fields = this.GetFields(table2, propertyNames);
                this.InsertNewPropertiesIntoQuery(fields);
            }

            this._query += $" LEFT JOIN {table2} ON {table1}.{property1Name} = {table2}.{property2Name}";

            return this;
        }

        /// <summary>
        /// Constructs the INSERT statement
        /// <para>Usage:</para>
        /// <para>.Insert&lt;Entity&gt;(dbConnection, x => new { x.Prop1, x.Prop2, x.Prop3, ... }, 5, "text", DateTime.Now)</para>
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table</typeparam>
        /// <param name="dbConnection">Used for determine the SQL sentence for retrieving the Identity value, given the DbConnection type</param>
        /// <param name="properties">Lambda Expression for the Entity Properties (Table Columns) for the INSERT statement</param>
        /// <param name="values">Values to be inserted</param>
        /// <returns>this</returns>
        public QueryBuilder Insert<T>(DbConnection dbConnection, Expression<Func<T, dynamic>> properties, params object[] values)
        {
            Type typeOfT = this.GetEntityType(properties.Parameters);
            var entity = Activator.CreateInstance(typeOfT);
            string table = this.GetEntityName(entity);

            string[] propertyNames = this.GetPropertyNamesFromDynamicExpression(entity, properties.Body);
            string[] stringValues = this.GetValues(values);

            this._query = $"INSERT INTO {table} ({string.Join(", ", propertyNames)}) VALUES ({string.Join(", ", stringValues)});";

            if (!propertyNames.Any())
                this._query = $"INSERT INTO {table} DEFAULT VALUES;";

            this._query += this._identityIds[dbConnection.GetType()];

            return this;
        }

        /// <summary>
        /// Constructs the UPDATE statement
        /// <para>Usage:</para>
        /// <para>.Update&lt;Entity&gt;(x => new { x.Prop1, x.Prop2, x.Prop3, ... }, 5, "text", DateTime.Now)</para>
        /// <para>(usually used followed by a .Where())</para>
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table</typeparam>
        /// <param name="properties">Lambda Expression for the Entity Properties (Table Columns) to be updated</param>
        /// <param name="values">Values to update</param>
        /// <returns>this</returns>
        public QueryBuilder Update<T>(Expression<Func<T, dynamic>> properties, params object[] values)
        {
            Type typeOfT = this.GetEntityType(properties.Parameters);
            var entity = Activator.CreateInstance(typeOfT);
            string table = this.GetEntityName(entity);

            string[] propertyNames = this.GetPropertyNamesFromDynamicExpression(entity, properties.Body);
            string[] stringValues = this.GetValues(values);

            string[] sentences = new string[] { };

            for (int i = 0; i < propertyNames.Length; i++)
                sentences = sentences.Append($"{propertyNames[i]} = {stringValues[i]}").ToArray();

            this._query = $"UPDATE {table} SET {string.Join(", ", sentences)}";

            return this;
        }

        /// <summary>
        /// Constructs the DELETE statement
        /// <para>Usage:</para>
        /// <para>.Delete(typeof(Entity))</para>
        /// <para>(usually used followed by a .Where())</para>
        /// </summary>
        /// <param name="entityType">Type of the Entity/Table</param>
        /// <returns>this</returns>
        public QueryBuilder Delete(Type entityType)
        {
            var entity = Activator.CreateInstance(entityType);
            string table = this.GetEntityName(entity);

            this._query = $"DELETE FROM {table}";

            return this;
        }

        /// <summary>
        /// Constructs the WHERE clause
        /// <para>Usage:</para>
        /// <para>.Where&lt;Entity&gt;(x => x.Prop, Operators.EqualTo, 5)</para>
        /// <para>(In case 'Operators.In' is used, value has to be an Array)</para>
        /// <para>(In case 'Operators.Like' is used, value has include '%' symbols)</para>
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the WHERE condition</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the condition</param>
        /// <param name="op">Condition Operator</param>
        /// <param name="value">Condition Value</param>
        /// <returns>this</returns>
        public QueryBuilder Where<T>(Expression<Func<T, object>> property, Operators op, object? value = null)
        {
            Type typeOfT = this.GetEntityType(property.Parameters);
            var entity = Activator.CreateInstance(typeOfT);
            string table = this.GetEntityName(entity);

            string propertyName = this.GetPropertyNameFromObjectExpression(entity, property.Body);
            string oper = this._operators[op];

            this._query += $" WHERE {table}.{propertyName} {oper}";

            if (value != null)
                this._query += $" {this.GetStringValue(value)}";

            this._isWhereClause = true;

            return this;
        }

        /// <summary>
        /// Constructs an AND clause to be used with a WHERE clause
        /// <para>Usage:</para>
        /// <para>.And&lt;Entity&gt;(x => x.Prop, Operators.EqualTo, 5)</para>
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the AND condition</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the condition</param>
        /// <param name="op">Condition Operator</param>
        /// <param name="openParenthesisBeforeCondition">Specifies whether an open parenthesis has to be included before the condition</param>
        /// <param name="closeParenthesisAfterCondition">Specifies whether a closed parenthesis has to be included after the condition</param>
        /// <param name="value">Condition Value</param>
        /// <returns>this</returns>
        public QueryBuilder And<T>(Expression<Func<T, object>> property, Operators op, bool openParenthesisBeforeCondition, bool closeParenthesisAfterCondition, object? value = null)
        {
            Type typeOfT = this.GetEntityType(property.Parameters);
            var entity = Activator.CreateInstance(typeOfT);
            string table = this.GetEntityName(entity);

            string propertyName = this.GetPropertyNameFromObjectExpression(entity, property.Body);
            string oper = this._operators[op];

            string openParenthesis = openParenthesisBeforeCondition ? "(" : "";
            string closeParenthesis = closeParenthesisAfterCondition ? ")" : "";
            this._query += $" AND {openParenthesis}{table}.{propertyName} {oper}";

            if (value != null)
                this._query += $" {this.GetStringValue(value)}";

            this._query += $"{closeParenthesis}";

            return this;
        }

        /// <summary>
        /// Constructs an OR clause to be used with a WHERE clause
        /// <para>Usage:</para>
        /// <para>.Or&lt;Entity&gt;(x => x.Prop, Operators.EqualTo, 5)</para>
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the OR condition</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the condition</param>
        /// <param name="op">Condition Operator</param>
        /// <param name="openParenthesisBeforeCondition">Specifies whether an open parenthesis has to be included before the condition</param>
        /// <param name="closeParenthesisAfterCondition">Specifies whether a closed parenthesis has to be included after the condition</param>
        /// <param name="value">Condition Value</param>
        /// <returns>this</returns>
        public QueryBuilder Or<T>(Expression<Func<T, object>> property, Operators op, bool openParenthesisBeforeCondition, bool closeParenthesisAfterCondition, object? value = null)
        {
            Type typeOfT = this.GetEntityType(property.Parameters);
            var entity = Activator.CreateInstance(typeOfT);
            string table = this.GetEntityName(entity);

            string propertyName = this.GetPropertyNameFromObjectExpression(entity, property.Body);
            string oper = this._operators[op];

            string openParenthesis = openParenthesisBeforeCondition ? "(" : "";
            string closeParenthesis = closeParenthesisAfterCondition ? ")" : "";
            this._query += $" OR {openParenthesis}{table}.{propertyName} {oper}";

            if (value != null)
                this._query += $" {this.GetStringValue(value)}";

            this._query += $"{closeParenthesis}";

            return this;
        }

        /// <summary>
        /// Add a close parenthesis
        /// </summary>
        /// <returns>this</returns>
        public QueryBuilder CloseParenthesis()
        {
            this._query += ")";

            return this;
        }

        /// <summary>
        /// Constructs an ORDER BY clause
        /// <para>Usage:</para>
        /// <para>.OrderBy&lt;Entity&gt;(x => x.Prop, Orders.Descending)</para>
        /// </summary>
        /// <typeparam name="T">Type of the Entity/Table for the ORDER BY</typeparam>
        /// <param name="property">Lambda Expression for the Entity Property (Table Column) for the ORDER BY</param>
        /// <param name="order">Ascending or Descending</param>
        /// <returns>this</returns>
        public QueryBuilder OrderBy<T>(Expression<Func<T, object>> property, Orders order)
        {
            Type typeOfT = this.GetEntityType(property.Parameters);
            var entity = Activator.CreateInstance(typeOfT);
            string table = this.GetEntityName(entity);

            string propertyName = this.GetPropertyNameFromObjectExpression(entity, property.Body);

            string orderString = order == Orders.Ascending ? "ASC" : "DESC";

            this._query += $" ORDER BY {table}.{propertyName} {orderString}";

            return this;
        }

        /// <summary>
        /// Constructs an OFFSET/LIMIT clause for Paging purposes
        /// <para>Usage:</para>
        /// <para>.Offset(dbConnection, 20, 10)</para>
        /// <para>(in this example we are getting the third page of ten items)</para>
        /// </summary>
        /// <param name="dbConnection">Used for determine the SQL sentence, given the DbConnection type</param>
        /// <param name="offset">Offset value</param>
        /// <param name="limit">Limit value</param>
        /// <returns>this</returns>
        public QueryBuilder Offset(DbConnection dbConnection, int offset, int limit)
        {
            string query = $" OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";

            if (dbConnection is SqliteConnection)
                query = $" LIMIT {limit} OFFSET {offset}";

            this._query += query;

            return this;
        }

        /// <summary>
        /// Returns the query
        /// </summary>
        /// <returns>Query</returns>
        public string Build()
        {
            return _query;
        }

        #region Private Methods

        private Type GetEntityType(IEnumerable<ParameterExpression> parametersExpression)
        {
            ParameterExpression parameterExpression = parametersExpression.FirstOrDefault();
            Type typeOfT = parameterExpression.Type;

            return typeOfT;
        }

        private string GetEntityName(object entity)
        {
            var attributes = (DescriptionAttribute[])entity.GetType().GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes[0].Description;
        }

        private string[] GetPropertyNamesFromDynamicExpression(object entity, Expression propertiesExpression)
        {
            var newExpression = (NewExpression)propertiesExpression;
            var propertyNames = new string[] { };

            var arguments = (IEnumerable<Expression>)newExpression.Arguments;

            foreach (var argument in arguments)
            {
                string propertyName = ((MemberExpression)argument).Member.Name;
                var attributes = (DescriptionAttribute[])entity.GetType().GetProperty(propertyName).GetCustomAttributes(typeof(DescriptionAttribute), false);

                propertyNames = propertyNames.Append(attributes[0].Description).ToArray();
            }

            return propertyNames;
        }

        private string GetPropertyNameFromObjectExpression(object entity, Expression propertyExpression)
        {
            MemberExpression memberExpression;

            try
            {
                var unaryExpression = (UnaryExpression)propertyExpression;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            catch (Exception)
            {
                memberExpression = (MemberExpression)propertyExpression;
            }

            string propertyName = memberExpression.Member.Name;
            var attributes = (DescriptionAttribute[])entity.GetType().GetProperty(propertyName).GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes[0].Description;
        }

        private string GetFields(string table, string[] propertyNames)
        {
            string[] aFields = new string[] { };

            foreach (var propertyName in propertyNames)
                aFields = aFields.Append($"{table}.{propertyName}").ToArray();

            string fields = string.Join(", ", aFields);

            return fields;
        }

        private void InsertNewPropertiesIntoQuery(string fields)
        {
            int fromIndex = _query.IndexOf("FROM");
            string fieldsToInsert = $", {fields}";
            this._query = this._query.Insert(fromIndex - 1, fieldsToInsert);
        }

        private string[] GetValues(object[] values)
        {
            string[] stringValues = new string[] { };

            foreach (var value in values)
                stringValues = stringValues.Append(this.GetStringValue(value)).ToArray();

            return stringValues;
        }

        private string GetStringValue(object value)
        {
            if (value == null)
                return string.Empty;

            Type typeOfValue = value.GetType();

            if (!typeOfValue.IsArray)
                return this._valueTypes[typeOfValue].Invoke(value);

            IEnumerable valueArray = (IEnumerable)value;
            string[] stringValues = new string[] { };

            foreach (var valueInArray in valueArray)
                stringValues = stringValues.Append(this.GetStringValue(valueInArray)).ToArray();

            string stringValue = String.Join(", ", stringValues);

            return $"({stringValue})";
        }

        #endregion
    }
}
