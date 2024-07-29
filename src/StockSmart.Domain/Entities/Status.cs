using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StockSmart.Domain.Entities
{
    public class Status : IEquatable<Status>
    {
        [JsonConstructor]
        private Status(int statusId, string name)
        {
            this.StatusId = statusId;
            this.Name = name;
        }

        public int StatusId { get; private set; }
        public string Name { get; private set; }
        public static Status Create(int statusId, string name) => new Status(statusId, name);

        #region Equatable
        public override bool Equals(object obj) => this.Equals(obj as Status);
        public bool Equals(Status other) => !(other is null) && this.Name == other.Name;
        public override int GetHashCode() => HashCode.Combine(this.Name);

        public static bool operator ==(Status left, Status right) => EqualityComparer<Status>.Default.Equals(left, right);
        public static bool operator !=(Status left, Status right) => !(left == right);
        #endregion
    }
}
