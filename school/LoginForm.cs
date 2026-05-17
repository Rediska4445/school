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

            txtLogin.Text = "Директор";
            txtPassword.Text = "director1";

            cmbPermission.SelectedIndex = 0; 
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
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRegPassword.Text))
            {
                MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtRegPassword.Focus();
                return;
            }

            byte? age = null;
            if (label1.Visible)
            {
                if (string.IsNullOrWhiteSpace(textBoxAge.Text))
                {
                    MessageBox.Show("Введите возраст", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxAge.Focus();
                    return;
                }

                if (byte.TryParse(textBoxAge.Text, out byte ageValue) && ageValue >= 5 && ageValue <= 100)
                {
                    age = ageValue;
                }
                else
                {
                    MessageBox.Show("Возраст должен быть от 5 до 100", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxAge.Focus();
                    return;
                }
            }

            string telephone = null;
            if (label2.Visible)
            {
                if (!string.IsNullOrWhiteSpace(textBoxTelephone.Text))
                {
                    telephone = textBoxTelephone.Text;
                }
            }

            int permissionID = cmbPermission.SelectedIndex + 1;

            int? classID = null;
            if (cmbClass.Visible && cmbClass.SelectedIndex >= 0)
            {
                classID = Convert.ToInt32(cmbClass.SelectedValue);
            }

            var application = RegistrationApplicationController._controller.Create(
                txtFullName.Text.Trim(),
                txtRegPassword.Text,
                permissionID,
                classID,
                age,
                telephone
            );

            if (application != null)
            {
                MessageBox.Show("Заявление на регистрацию успешно отправлено! Ожидайте одобрения от директора.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtFullName.Clear();
                txtRegPassword.Clear();
                textBoxAge.Clear();
                textBoxTelephone.Clear();
                cmbPermission.SelectedIndex = -1;
                cmbClass.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Ошибка при создании заявления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
