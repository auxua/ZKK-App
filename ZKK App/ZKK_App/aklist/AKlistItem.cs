using System;
using System.Collections.Generic;
using System.Text;

namespace ZKK_App.aklist
{
    /// <summary>
    /// Just a small AKlistItem-Class for being a Model in Model-View-Data for the workshopplan
    /// Is Queatebly and Comparable (making implicit sorting possible!)
    /// </summary>
    public class AKlistItem : IEquatable<AKlistItem>, IComparable<AKlistItem>
    {
        public AKlistItem() { }

        public string Title { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string Room { get; set; }

        public override string ToString()
        {
            return "Title: " + Title + "   Day: " + Day + "  Time: " + Time + "  Room: " + Room;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            AKlistItem objAsPart = obj as AKlistItem;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public int SortByNameAscending(string name1, string name2)
        {
            return name1.CompareTo(name2);
        }

        /// <summary>
        /// The Comparer compares in this order:
        ///     * Day
        ///     * Time
        /// </summary>
        public int CompareTo(AKlistItem comparePart)
        {
            // A null value means that this object is greater.
            if (comparePart == null)
            {
                return 1;
            }
            else
            {
                if (this.Day == comparePart.Day)
                {
                    return this.Time.CompareTo(comparePart.Time);
                }
                else
                {
                    return this.Day.CompareTo(comparePart.Day);
                }
            }
                
        }
        public override int GetHashCode()
        {
            return (Day+Time+Room+Title).GetHashCode();
        }

        /// <summary>
        /// Compare element by element
        /// </summary>
        public bool Equals(AKlistItem other)
        {
            if (other == null) return false;
            if (this.Title == other.Title)
            {
                if (this.Time == other.Time)
                {
                    if (this.Room == other.Room)
                    {
                        if (this.Day == other.Day)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
