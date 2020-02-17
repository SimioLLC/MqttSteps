using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MqttSteps
{
    public sealed class AlertBox
    {
        private static readonly AlertBox instance = new AlertBox();

        static FormAlert MyForm;

        static AlertBox()
        {
        }

        private AlertBox()
        {
            MyForm = new FormAlert();
        }

        public static DialogResult ShowMessage(string message)
        {
            MyForm.Message = message;
            DialogResult result = MyForm.ShowDialog();
            return result;
        }

        public static AlertBox Instance
        {
            get { return instance; }
        }

    }
}
