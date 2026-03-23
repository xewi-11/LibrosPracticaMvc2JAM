using LibrosPracticaMvc2JAM.Extensions;

namespace LibrosPracticaMvc2JAM.Helpers
{
    public class SessionHelper
    {
        private const string USER_Session_Key = "UserSession";

        public static List<int> GetCarrito(ISession session)
        {
            var carrito = session.GetObject<List<int>>(USER_Session_Key);
            if (carrito != null)
            {
                return carrito;
            }
            else
            {
                return null;
            }
        }
        public static void SetCarrito(ISession session, List<int> carrito)
        {
            session.SetObject(USER_Session_Key, carrito);
        }
        public static void LimpiarCarrito(ISession session)
        {
            session.Remove(USER_Session_Key);
        }

    }
}
