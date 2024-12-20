using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaiTapBuoi6_Winform.Models;

namespace BaiTapBuoi6_Winform
{
    public partial class Form1 : Form
    {
        private StudentDBContext context;
        private Student SelectedStudent;

        public Form1()
        {
            InitializeComponent();
        }
        private void loadData()
        {
            context = new StudentDBContext();
            List<Student> lstStudent = context.Students.ToList();
            List < Faculty> lstFaculty = context.Faculties.ToList();
            fillFacultyComboBox(lstFaculty);
            BlindGrid(lstStudent);
            
        }
        private void ClearForm()
        { 
            txtMSSV.Clear();
            txtHoTen.Clear();
            txtDtb.Clear();
            CbFalculty.SelectedIndex = -1;
        }
        private void BlindGrid(List<Student> lstStudent)
        {
            dtgSinhVien.Rows.Clear();
            foreach(var item in lstStudent  )
            {
                int index = dtgSinhVien.Rows.Add();
                dtgSinhVien.Rows[index].Cells[0].Value = item.StudentID;
                dtgSinhVien.Rows[index].Cells[1].Value= item.FullName;
               dtgSinhVien.Rows[index].Cells[2].Value=item.Faculty.FacultyName;
                dtgSinhVien.Rows[index].Cells[3].Value=item.AverageScore;

            }

        }
        private void fillFacultyComboBox (List<Faculty> lstfaculty)
        {
            CbFalculty.DataSource = lstfaculty;
            CbFalculty.ValueMember = "FacultyID";
            CbFalculty.DisplayMember = "FacultyName";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void dtgSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dtgSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgSinhVien.Rows[e.RowIndex];
                string studentId = row.Cells[0].Value.ToString();
                SelectedStudent = context.Students.FirstOrDefault(s => s.StudentID == studentId);
                if (studentId != null)
                {
                    txtMSSV.Text = SelectedStudent.StudentID.ToString();
                    txtHoTen.Text = SelectedStudent.FullName;
                    CbFalculty.SelectedValue = SelectedStudent.FacultyID;
                    txtDtb.Text = SelectedStudent.AverageScore.ToString();


                }

            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                Student student = new Student()
                {
                    StudentID = txtMSSV.Text,
                    FullName = txtHoTen.Text,
                    FacultyID = Convert.ToInt32(CbFalculty.SelectedValue),
                    AverageScore = float.Parse(txtDtb.Text)
                };
                context.Students.Add(student);
                context.SaveChanges();
                ClearForm();
                loadData();
                MessageBox.Show("Ban da them thanh cong");

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (SelectedStudent != null)
            {
                try
                {
                    SelectedStudent.StudentID = txtMSSV.Text;
                    SelectedStudent.FullName = txtHoTen.Text;
                    SelectedStudent.AverageScore = float.Parse(txtDtb.Text);
                    SelectedStudent.FacultyID = Convert.ToInt32(CbFalculty.SelectedValue);

                    context.SaveChanges();
                    ClearForm() ;
                    loadData();
                    MessageBox.Show("Ban da cap nhat thanh cong");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Ban Can Chon De Sua");
            }

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (SelectedStudent != null)
            {
                try
                {
                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên có mã số " + SelectedStudent.StudentID + " ?", "Thông báo", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        context.Students.Remove(SelectedStudent);
                        context.SaveChanges();

                        MessageBox.Show("Xóa sinh viên thành công.");
                        ClearForm();
                        loadData();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa.");
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Ban co chac chan thoat !", "Xac Nhan", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if(rs == DialogResult.Yes)
            {
                this.Close();
            }

        }
    }
}
