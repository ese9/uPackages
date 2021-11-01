using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace NineGames.Storage
{
    [Serializable]
    public readonly struct DataVersion : IComparable<DataVersion>, IEquatable<DataVersion>, ISerializable
    {
        public static readonly DataVersion MinValue = new DataVersion(0, 0);
        public static readonly DataVersion MaxValue = new DataVersion(int.MaxValue, int.MaxValue);

        public readonly int Major;
        public readonly int Minor;

        public DataVersion(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }

        private DataVersion(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            var version = info.GetString("version").Split('.');

            Major = int.Parse(version[0]);
            Minor = int.Parse(version[1]);
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue("version", $"{Major}.{Minor}");
        }

        public override string ToString() => $"{Major}.{Minor}";

        public int CompareTo(DataVersion other)
        {
            var difMajors = Major.CompareTo(other.Major);
            if (difMajors == 0)
                return Minor.CompareTo(other.Minor);

            return difMajors;
        }

        public bool Equals(DataVersion other) => Major == other.Major && Minor == other.Minor;
        public override bool Equals(object obj) => obj is DataVersion other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (Major * 397) ^ Minor;
            }
        }

        public static DataVersion Parse(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                throw new InvalidCastException(version);

            try
            {
                var splitString = version.Split('.');

                if (splitString.Length < 2)
                    throw new InvalidCastException($"Can't cast {version} to type DataVersion");

                if (int.TryParse(splitString[0], out var major) && int.TryParse(splitString[1], out var minor))
                    return new DataVersion(major, minor);

                throw new InvalidCastException($"Can't cast {version} to type DataVersion");
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException($"Can't cast {version} to type DataVersion");
            }
        }

        public static bool TryParse(string version, out DataVersion dataVersion)
        {
            try
            {
                dataVersion = Parse(version);
                return true;
            }
            catch (InvalidCastException)
            {
                dataVersion = MinValue;
                return false;
            }
        }

        public static bool operator >(DataVersion a, DataVersion b) => a.CompareTo(b) > 0;
        public static bool operator <(DataVersion a, DataVersion b) => a.CompareTo(b) < 0;
        public static bool operator >=(DataVersion a, DataVersion b) => a.CompareTo(b) >= 0;
        public static bool operator <=(DataVersion a, DataVersion b) => a.CompareTo(b) <= 0;
        public static bool operator ==(DataVersion a, DataVersion b) => a.Equals(b);
        public static bool operator !=(DataVersion a, DataVersion b) => !(a == b);
    }
}