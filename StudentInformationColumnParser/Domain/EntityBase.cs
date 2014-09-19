using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommitParser.Domain
{
    /// <summary>
    /// Base class for entities that must have an identifier attribute. If Identity Type is Guid, a new one is auto generated here.
    /// </summary>
    /// <typeparam name="TIdentityType">The type of the identifier attribute for the entity</typeparam>
    [Serializable]
    public abstract class EntityBase<TIdentityType> : IEquatable<EntityBase<TIdentityType>> where TIdentityType : struct
    {
        private object _id;

        [NotMapped]
        public object ObjectId { get { return _id; } }

        [Key]
        [Column(Order = 1)]
        public virtual TIdentityType Id
        {
            get
            {
                if (_id == null && typeof(TIdentityType) == typeof(Guid))
                    _id = Guid.NewGuid();

                return _id == null ? default(TIdentityType) : (TIdentityType)_id;
            }

            protected set { _id = value; }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 23 + ((_id != null) ? this._id.GetHashCode() : 0);
                return result;
            }
        }

        [NotMapped]
        public bool IsNew { get { return Id.Equals(default(TIdentityType)); } }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(EntityBase<TIdentityType>)) return false;

            return Equals((EntityBase<TIdentityType>)obj);
        }

        public bool Equals(EntityBase<TIdentityType> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;

            return other.Id.Equals(Id);
        }

        public static bool operator ==(EntityBase<TIdentityType> left, EntityBase<TIdentityType> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EntityBase<TIdentityType> left, EntityBase<TIdentityType> right)
        {
            return !Equals(left, right);
        }
    }
}
