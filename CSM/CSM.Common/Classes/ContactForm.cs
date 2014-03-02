using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSM.Classes
{
    public class ContactForm
    {
        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        private string asunto;

        public string Asunto
        {
            get { return asunto; }
            set { asunto = value; }
        }
        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private string mensaje;

        public string Mensaje
        {
            get { return mensaje; }
            set { mensaje = value; }
        }

        public override string ToString()
        {
            return string.Format("Nombre: {0}\nEmail: {1}\nAsunto: {2}\nMensaje: {3}",Nombre,Email,Asunto,Mensaje);
        }

    }
}
