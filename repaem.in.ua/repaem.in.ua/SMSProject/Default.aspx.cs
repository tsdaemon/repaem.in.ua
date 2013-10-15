using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMSProject.Services;

namespace SMSProject
{
    public partial class _Default : System.Web.UI.Page
    {
        // информация о текущей операции
        private operations CurrentOperation
        {
            get
            {
                return ViewState["CurrOperation"] == null ? operations.UnDef : (operations)ViewState["CurrOperation"];
            }
            set
            {
                ViewState["CurrOperation"] = value;
            }
        }
        private enum operations { UnDef = -1, Auth = 0, CreditBalance = 1, SendSMS = 2, NewMessages = 3, MessageStatus= 4 };
        private SMSWorker worker;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                SetPaneVisible(null);
            }
            if (worker == null)
                worker = SMSWorker.GetInstance();
        }

        #region veb methods
        // Авторизируемся на сервере
        private void Auth()
        {
            try
            {
                string res  = worker.Auth(idLogin.Text, idPass.Text);
                if (res == "Неверный логин или пароль")
                    worker.CloseSession();
                lResult.Text = res;
            }
            catch (Exception e)
            {
                lResult.Text = "Не удалось авторизоваться! " + e.Message;
            }
        }
        // Получаем остаток
        private void CreditBalance()
        {
            try
            {
                lResult.Text = String.Format("Остаток на счете {0} !", worker.GetCreditBalance());
            }
            catch (Exception e)
            {
                lResult.Text = "Не удалось получить баланс счета! " + e.Message;
            }
        }
        // Отправляем смс
        private void SendSms()
        {
            string resStr = "";
            try
            {
                string[] res = worker.SendSMS(idSender.Text, idReceivers.Text, idText.Text, idWap.Text);
                if (res.Length == 2)
                    resStr = String.Format("{0} {1}", res[0], res[1]);
                else
                    resStr = res[0];
            }
            catch (Exception e)
            {
                resStr = "Не удалось отправить sms! " + e.Message;
            }
            lResult.Text = resStr;
        }
        // возвращает массив сообщений с неизвестным статусом 
        private void NewMessages()
        {
            string resStr = "";
            try
            {
                string[] str = worker.GetNewMessages();
                foreach (string s in str)
                    if ((s != null) && (s.Length > 0))
                        resStr += s + "<br />";
            }
            catch (Exception e)
            {
                resStr = "Не удалось получить массив сообщений! " + e.Message;
            }
            lResult.Text = resStr;
        }
        // возвращает статус доставки сообщения 
        private void GetMessageStatus()
        {
            try
            {
                lResult.Text = worker.GetMessageStatus(idMessageID.Text);
            }
            catch (Exception e)
            {
                lResult.Text = "Не удалось получить статус доставки сообщения! " + e.Message;
            }
        }
        #endregion

        /// <summary>
        ///     Делаем панель видимой, остальные - скрываем
        /// </summary>
        /// <param name="panel"></param>
        private void SetPaneVisible(Panel panel)
        {
            foreach (Control c in idForm.Controls)
                if (c is Panel)
                    if (panel != null)
                        ((Panel)c).Visible = (c as Panel) == panel ? true : false;
                    else
                        ((Panel)c).Visible = false;
        }

        /// <summary>
        ///     Обработчик нажатий на кнопки меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void idMainMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            lResult.Text = "";
            switch (e.Item.Value)
            {
                case "Auth":
                    SetPaneVisible(pAuth);
                    CurrentOperation = operations.Auth;
                    break;
                case "CreditBalance":
                    SetPaneVisible(pGetBalance);
                    CurrentOperation = operations.CreditBalance;
                    break;
                case "SendSMS":
                    SetPaneVisible(pSendSMS);
                    CurrentOperation = operations.SendSMS;
                    break;
                case "NewMessages":
                    SetPaneVisible(pNewMessages);
                    CurrentOperation = operations.NewMessages;
                    break;
                case "MessageStatus":
                    SetPaneVisible(pMessageStatus);
                    CurrentOperation = operations.MessageStatus;
                    break;
                default: 
                    break;
            }
        }

        /// <summary>
        ///     Обработчик кнопки работы с сервисом
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bOk_Click(object sender, EventArgs e)
        {
            switch (CurrentOperation)
            {
                case operations.Auth: Auth(); break;
                case operations.CreditBalance: CreditBalance(); break;
                case operations.SendSMS: SendSms(); break;
                case operations.NewMessages: NewMessages(); break;
                case operations.MessageStatus: GetMessageStatus(); break;
                case operations.UnDef:
                    lResult.Text = "Выберите операцию!";
                    break;
            }
        }


    }
}
