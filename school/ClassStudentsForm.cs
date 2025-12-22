using school.Controllers;
using school.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace school
{
    public partial class ClassStudentsForm : Form
    {
        public User ReturnedStudent { get; private set; }  // ✅ Возвращаемый ученик
        public User SelectedStudent { get; set; }          // ✅ Предвыделенный

        private TabControl tabControlClasses;
        private DataGridView dataGridViewStudents;
        private ClassController classController;

        public ClassStudentsForm()
        {
            classController = ClassController._controller; 
            InitializeComponent();
            SetupForm();
            LoadClasses();
        }

        private void InitializeComponent()
        {
            this.Text = "Ученики по классам";
            this.Size = new Size(800, 650);

            Panel panelButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.LightGray
            };

            Button btnSelect = new Button
            {
                Text = "Выбрать",
                Location = new Point(10, 10),
                Size = new Size(100, 30),
                DialogResult = DialogResult.OK
            };
            panelButtons.Controls.Add(btnSelect);

            Panel panelMain = new Panel { Dock = DockStyle.Fill };
            tabControlClasses = new TabControl
            {
                Dock = DockStyle.Fill
            };

            dataGridViewStudents = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };
            dataGridViewStudents.DataError += (s, e) => { e.Cancel = true; };

            panelMain.Controls.Add(tabControlClasses);
            this.Controls.Add(panelButtons);
            this.Controls.Add(panelMain);

            tabControlClasses.SelectedIndexChanged += TabControlClasses_SelectedIndexChanged;
            btnSelect.Click += BtnSelect_Click;
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (dataGridViewStudents.CurrentRow != null)
            {
                var selectedStudent = (User)dataGridViewStudents.CurrentRow.Tag;
                this.ReturnedStudent = selectedStudent;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Выберите ученика в таблице!");
            }
        }

        private void SetupForm()
        {
            dataGridViewStudents.Columns.Clear();

            dataGridViewStudents.Columns.Add("colUserID", "ID");
            dataGridViewStudents.Columns["colUserID"].Visible = false;
            dataGridViewStudents.Columns.Add("colFullName", "ФИО ученика");
            dataGridViewStudents.Columns.Add("colClassName", "Класс");
        }

        private void DataGridViewStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridViewStudents.Columns["colSelect"].Index)
            {
                dataGridViewStudents.CurrentCell = dataGridViewStudents.Rows[e.RowIndex].Cells[1];
            }
        }

        private void LoadClasses()
        {
            var classes = classController.GetAllClasses();

            foreach (var classItem in classes)
            {
                TabPage tabPage = new TabPage(classItem.ClassName)
                {
                    Tag = classItem.ClassID 
                };

                tabControlClasses.TabPages.Add(tabPage);
            }

            if (tabControlClasses.TabCount > 0)
            {
                LoadStudentsForClass((int)tabControlClasses.TabPages[0].Tag);
            }
        }

        private void TabControlClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlClasses.SelectedTab != null)
            {
                int classId = (int)tabControlClasses.SelectedTab.Tag;

                if (!tabControlClasses.SelectedTab.Controls.Contains(dataGridViewStudents))
                {
                    tabControlClasses.SelectedTab.Controls.Add(dataGridViewStudents);
                    dataGridViewStudents.Dock = DockStyle.Fill;
                }

                LoadStudentsForClass(classId);
            }
        }

        private void LoadStudentsForClass(int classId)
        {
            dataGridViewStudents.SuspendLayout();

            try
            {
                dataGridViewStudents.Rows.Clear();

                var students = UserController._userController.GetClassStudents(classId);

                foreach (var student in students)
                {
                    int rowIndex = dataGridViewStudents.Rows.Add();
                    var row = dataGridViewStudents.Rows[rowIndex];

                    row.Cells["colUserID"].Value = student.UserID;
                    row.Cells["colFullName"].Value = student.FullName;
                    row.Cells["colClassName"].Value = student.Class?.ClassName ?? "";

                    row.Tag = student;

                    if (SelectedStudent != null && student.UserID == SelectedStudent.UserID)
                    {
                        dataGridViewStudents.CurrentCell = row.Cells[1];
                        dataGridViewStudents.Rows[rowIndex].Selected = true;
                    }
                }
            }
            finally
            {
                dataGridViewStudents.ResumeLayout();
                dataGridViewStudents.Refresh();
            }
        }
    }
}
