using System.ComponentModel;

namespace System.Web.Mvc
{
    public interface IHideMembers
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object value);

        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

    }
}