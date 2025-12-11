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

        // Существующие записи для тестов
//        7	Борисова Дарья  hashed_pass8 Ученик	5А
//      8	Васильев Максим hashed_pass9 Ученик	5Б
//      9	Григорьева Елена    hashed_pass10 Ученик	6А
//      10	Дмитриев Сергей hashed_pass11 Ученик	7А
//      11	Егорова Катя    hashed_pass12 Ученик	7Б
//      6	Кузнецова Виктория  pass9a1 Ученик	5А
//      4	Козлова Мария Ивановна hashed_pass4    Учитель	6Б
//      2	Петрова Анна Сергеевна hashed_pass2    Учитель	5Б
//      3	Сидоров Петр Петрович hashed_pass3    Учитель	6А
//      5	Смирнова Ольга Васильевна hashed_pass5    Учитель	7А
//      1	Иванов Иван Иванович hashed_pass1    Директор	5А

        public LoginForm()
        {
            InitializeComponent();

            txtLogin.Text = "Иванов Иван Иванович";
            txtPassword.Text = "hashed_pass1";
        }

        private void btnLogin_Click(object sender, EventArgs e)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !IsLoggedIn)
            {
                DialogResult result = MessageBox.Show("Выйти из приложения?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
            txtLogin.Focus();
        }
    }
}
