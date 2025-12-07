using school.Controllers;
using school.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace school
{
    public partial class LoginForm : Form
    {
        public static bool IsLoggedIn { get; private set; }

        public LoginForm()
        {
            InitializeComponent();

            txtLogin.Text = "Кузнецова Виктория";
            txtPassword.Text = "pass9a1";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            // ✅ Валидация ввода
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("[translate:Введите логин и пароль!]", "[translate:Ошибка]",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return;
            }

            // ✅ Аутентификация через UserController
            Cursor = Cursors.WaitCursor;
            try
            {
                User user = UserController._userController.GetUserByLoginPassword(login, password);

                if (user != null)
                {
                    // ✅ УСПЕШНЫЙ ВХОД
                    UserController.CurrentUser = user;
                    IsLoggedIn = true;

                    MessageBox.Show($"[translate:Добро пожаловать, {user.FullName}!]",
                        "[translate:Успешный вход]", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ Открываем главную форму
                    Form1 mainForm = new Form1();
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    // ❌ НЕВЕРНЫЕ ДАННЫЕ
                    MessageBox.Show("[translate:Неверный логин или пароль!]", "[translate:Ошибка входа]",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtLogin.SelectAll();
                    txtLogin.Focus();
                }
            }
            catch (Exception ex)
            {
                // ❌ ОШИБКА БД
                MessageBox.Show($"[translate:Ошибка подключения к базе данных: {ex.Message}]",
                    "[translate:Ошибка]", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !IsLoggedIn)
            {
                DialogResult result = MessageBox.Show("[translate:Выйти из приложения?]",
                    "[translate:Подтверждение]", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            txtLogin.Focus(); // ✅ Фокус на логин при запуске
        }
    }
}
