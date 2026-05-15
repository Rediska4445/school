using school.Controllers;
using school.Models;
using System;
using System.Windows.Forms;

namespace school
{
    public partial class LoginForm : Form
    {
        public static bool IsLoggedIn { get; private set; }

        public LoginForm()
        {
            var settings = AppSettings.Load();
            Form1.CONNECTION_STRING = settings.ConnectionString;
            Controller.DATABASE_NAME = settings.DatabaseName;

            FileLogger.logger.Debug("DB: " + Controller.DATABASE_NAME);

            Controller.sqlController.PrepareDatabase(settings.MasterConnectionString, Controller.DATABASE_NAME);
            Controller.sqlController.PrepareTestData(settings.MasterConnectionString, Controller.DATABASE_NAME);

            InitializeComponent();

            txtLogin.Text = "Петрова Анна Сергеевна";
            txtPassword.Text = "teacher2";
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return;
            }

            Cursor = Cursors.WaitCursor;
            try
            {
                User user = UserController._userController.GetUserByLoginPassword(login, password);

                if (user != null)
                {
                    UserController.CurrentUser = user;
                    IsLoggedIn = true;

                    MessageBox.Show($"Добро пожаловать, {user.FullName}!",
                        "Успешный вход", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Form1 mainForm = new Form1();
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка входа",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtLogin.SelectAll();
                    txtLogin.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string password = txtRegPassword.Text;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните ФИО и пароль!", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbPermission.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите роль!", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int permissionID = cmbPermission.SelectedIndex + 1;
            int? classID = cmbClass.SelectedIndex >= 0 ? cmbClass.SelectedIndex + 1 : (int?)null;

            bool success = UserController._userController.RegisterUserWithMessage(fullName, password, permissionID, classID);

            if (success)
            {
                txtFullName.Clear();
                txtRegPassword.Clear();
                cmbPermission.SelectedIndex = -1;
                cmbClass.SelectedIndex = -1;
            }
        }
    }
}
