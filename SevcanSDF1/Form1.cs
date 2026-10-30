using SevcanSDF1;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SevcanSDF1

{
    public partial class Form1 : Form
    {
        private EmployeeServiceBase service = new EmployeeService();

        public Form1()
        {
            InitializeComponent();

            var lblInfo = this.Controls.Find("lblStatInfo", true).FirstOrDefault() as Label;
            if (lblInfo != null)
            {
                lblInfo.Text = "Axtarılacaq işçinin kodunu daxil edin.";
            }
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            var txtCode = this.Controls.Find("txtEmpCode", true).FirstOrDefault() as TextBox;
            var txtFirstName = this.Controls.Find("txtEmpFirstName", true).FirstOrDefault() as TextBox;
            var txtLastName = this.Controls.Find("txtEmpLastName", true).FirstOrDefault() as TextBox;
            var txtAge = this.Controls.Find("txtEmpAge", true).FirstOrDefault() as TextBox;

            if (txtCode == null || txtFirstName == null || txtLastName == null || txtAge == null)
            {
                MessageBox.Show("İnterfeys elementləri tapılmadı! Adlandırmanı yoxlayın.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string code = txtCode.Text.Trim();
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string ageText = txtAge.Text.Trim();

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(firstName) ||
                string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(ageText))
            {
                MessageBox.Show("Zəhmət olmasa bütün sahələri doldurun!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(ageText, out int age) || age <= 0)
            {
                MessageBox.Show("Yaş düzgün daxil edilməyib!", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var newEmp = new Employee
            {
                Code = code,
                FirstName = firstName,
                LastName = lastName,
                Age = age
            };

            bool success = service.AddEmployee(newEmp);

            if (success)
            {
                MessageBox.Show("İşçi uğurla əlavə olundu!", "Uğurlu", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtCode.Clear();
                txtFirstName.Clear();
                txtLastName.Clear();
                txtAge.Clear();

                RefreshEmployeeGrid();
            }
            else
            {
                MessageBox.Show("Bu koda malik işçi artıq mövcuddur!", "Təkrar Kod", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAddPayment_Click(object sender, EventArgs e)
        {
            var txtCode = this.Controls.Find("txtPayEmpCode", true).FirstOrDefault() as TextBox;
            var txtTitle = this.Controls.Find("txtPayTitle", true).FirstOrDefault() as TextBox;
            var txtAmount = this.Controls.Find("txtPayAmount", true).FirstOrDefault() as TextBox;

            if (txtCode == null || txtTitle == null || txtAmount == null)
            {
                MessageBox.Show("İnterfeys elementləri tapılmadı!", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string code = txtCode.Text.Trim();
            string title = txtTitle.Text.Trim();
            string amountText = txtAmount.Text.Trim();

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(amountText))
            {
                MessageBox.Show("Bütün ödəniş məlumatlarını doldurun!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(amountText, out double amount))
            {
                MessageBox.Show("Məbləğ düzgün daxil edilməyib!", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var employee = service.FindEmployee(code);
            if (employee == null)
            {
                MessageBox.Show("Belə koda malik işçi tapılmadı! Əvvəlcə işçini əlavə edin.", "Tapılmadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            employee.Payments.Add(new Payment { Title = title, Amount = amount });

            MessageBox.Show("Ödəniş işçiyə uğurla əlavə olundu!", "Uğurlu", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtCode.Clear();
            txtTitle.Clear();
            txtAmount.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var txtCode = this.Controls.Find("txtSearchCode", true).FirstOrDefault() as TextBox;
            var lblInfo = this.Controls.Find("lblStatInfo", true).FirstOrDefault() as Label;
            var grid = this.Controls.Find("dgvStatPayments", true).FirstOrDefault() as DataGridView;

            if (txtCode == null)
            {
                MessageBox.Show("Axtarış xanası tapılmadı!", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string code = txtCode.Text.Trim();

            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Zəhmət olmasa axtarış üçün işçi kodu daxil edin!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var emp = service.FindEmployee(code);

            if (emp == null)
            {
                MessageBox.Show("Belə koda malik işçi tapılmadı!", "Tapılmadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (lblInfo != null) lblInfo.Text = "İşçi tapılmadı.";
                if (grid != null) grid.DataSource = null;
                return;
            }

            if (lblInfo != null)
                lblInfo.Text = $"Kod: {emp.Code} | Ad Soyad: {emp.FirstName} {emp.LastName} | Yaş: {emp.Age}";

            if (grid != null)
            {
                grid.DataSource = null;
                grid.AutoGenerateColumns = true;
                grid.DataSource = emp.Payments;
            }
        }

        private void RefreshEmployeeGrid()
        {
            var grid = this.Controls.Find("dgvEmployees", true).FirstOrDefault() as DataGridView;
            if (grid != null)
            {
                grid.DataSource = null;
                grid.DataSource = service.GetAllEmployees();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Proqramdan çıxmaq istədiyinizə əminsinizmi?", "Çıxış Təsdiqi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnAddEmployee_Click_1(object sender, EventArgs e) => btnAddEmployee_Click(sender, e);
        private void btnAddPayment_Click_1(object sender, EventArgs e) => btnAddPayment_Click(sender, e);
        private void btnSearch_Click_1(object sender, EventArgs e) => btnSearch_Click(sender, e);
        private void btnExit_Click_1(object sender, EventArgs e) => btnExit_Click(sender, e);

        private void txtEmpLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEmpCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblStatInfo_Click(object sender, EventArgs e)
        {

        }

        private void txtEmpAge_TextChanged(object sender, EventArgs e)
        {

        }
    }
}