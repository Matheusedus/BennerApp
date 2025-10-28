using BennerApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BennerApp.Resources
{
    // habilita o botão se a transição fizer sentido
    // param: "Pago", "Enviado", "Recebido"
    public class StatusEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is StatusPedido atual) || parameter == null) return false;
            var alvo = parameter.ToString();

            if (alvo == "Pago") return atual == StatusPedido.Pendente;
            if (alvo == "Enviado") return atual == StatusPedido.Pago;
            if (alvo == "Recebido") return atual == StatusPedido.Enviado;

            return false;
        }
        public object ConvertBack(object value, Type t, object p, CultureInfo c) => throw new NotImplementedException();
    }
}
