﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PathPointer
{
    public partial class AddEmployments : Form
    {
        Employments employments = new EmploymentsGoals();
        public static BindingList<DataManagement> varCells;


        public AddEmployments()
        {
            InitializeComponent();
        }

        private void Employments_Load(object sender, EventArgs e)
        {
            BtnGoals_Click(null, null);
            SetDisign();
        }

        private void BtnBusiness_Click(object sender, EventArgs e)
        {
            employments = new EmploymentsBusiness();
            FillGrid();
        }

        public void BtnGoals_Click(object sender, EventArgs e)
        {
            employments = new EmploymentsGoals();
            FillGrid();

        }

        private void BtnRest_Click(object sender, EventArgs e)
        {
            employments = new EmploymentsRest();
            FillGrid();
        }

        private void BtnFun_Click(object sender, EventArgs e)
        {
            employments = new EmploymentsFun();
            FillGrid();
        }

        private void AddButton_Click(object sender, EventArgs e)   
        {
            Form empForm;
            switch (employments.EmpType)
            {
                case "Business":
                    empForm = new AddBusy();
                    AddSchedule.Schedule = null;
                    AddBusy.BusyName = null;
                    break;
                case "Goals":
                    empForm = new AddGoal();
                    break;
                case "Rest":
                    empForm = new AddRest();
                    break;
                default:
                    empForm = new AddFun();
                    break;
            }

            MenuManagement.ShowForm(this, empForm);
        }

        public void FillGrid() {
            dataGridBusiness.DataSource = employments.FillGrid(ref varCells).DataSource;
            if (dataGridBusiness[0, 0].Value.ToString() != Texts.emptyEmpFile)
            {
                dataGridBusiness.ReadOnly = false;
                dataGridBusiness.CurrentCell = dataGridBusiness[0, 0];
                dataGridBusiness.Focus();
            }
            else dataGridBusiness.ReadOnly = true;
        }

        private void BackMain_Click(object sender, EventArgs e)
        {
            MainMenu mainMenu = MainMenu.CreateInstance(this);
        }
        
        private void dataGridBusiness_KeyDown(object sender, KeyEventArgs e)
        {
            string cellVal = dataGridBusiness.CurrentCell.Value.ToString();    //передача в cellVal значения выбранной ячейки
            
            if (e.KeyCode == Keys.Delete)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить \"{cellVal}\"", "Вы уверены?", MessageBoxButtons.YesNo);


                if (result == DialogResult.Yes)
                {
                    varCells.RemoveAt(dataGridBusiness.CurrentCell.RowIndex);
                    employments.DeleteEmpFromFile(cellVal);
                } 
            }


            FillGrid();
        }



        private void dataGridBusiness_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridBusiness.CurrentCell.Value != null)
            {
                string editedEmp = dataGridBusiness.CurrentCell.Value.ToString();
                editedEmp = employments.CheckEmploymentFormat(editedEmp);
                if (editedEmp != "") employments.EditEmpFiles(editedEmp, dataGridBusiness.CurrentCell.RowIndex);
            }
            FillGrid();
        }

        private void Employments_FormClosing(object sender, FormClosingEventArgs e)
        {           
            MenuManagement.HideForm(this, e);
        }



        private void SetDisign() {

            EmploymentsBusiness business = new EmploymentsBusiness();
            EmploymentsGoals goals = new EmploymentsGoals();
            EmploymentsRest rest = new EmploymentsRest();
            EmploymentsFun fun = new EmploymentsFun();

            BtnBusiness.BackColor = business.CellColor;
            BtnGoals.BackColor = goals.CellColor;
            BtnRest.BackColor = rest.CellColor;
            BtnFun.BackColor = fun.CellColor;

        }



    }
}
