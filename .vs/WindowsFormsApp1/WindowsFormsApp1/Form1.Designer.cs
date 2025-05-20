using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private DataStorage data = new DataStorage();
        private string filePath = "data.json";

      

        private void buttonAddExpense_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBoxAmount.Text, out decimal amount) &&
                !string.IsNullOrWhiteSpace(textBoxCategory.Text))
            {
                data.Expenses.Add(new Expense
                {
                    Category = textBoxCategory.Text,
                    Amount = amount,
                    Date = dateTimePicker.Value
                });

                UpdateGrid();
                UpdateBalance();
                ClearInputs();
            }
            else
            {
                MessageBox.Show("Введите корректную сумму и категорию.");
            }
        }

        private void buttonAddIncome_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBoxAmount.Text, out decimal amount))
            {
                data.Incomes.Add(new Income
                {
                    Amount = amount,
                    Date = dateTimePicker.Value
                });

                UpdateGrid();
                UpdateBalance();
                ClearInputs();
            }
            else
            {
                MessageBox.Show("Введите корректную сумму дохода.");
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(data, Formatting.Indented));
                MessageBox.Show("Данные успешно сохранены.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    data = JsonConvert.DeserializeObject<DataStorage>(json) ?? new DataStorage();
                    UpdateGrid();
                    UpdateBalance();
                    MessageBox.Show("Данные успешно загружены.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Файл не найден.");
            }
        }

        private void UpdateGrid()
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            dataGridView.Columns.Add("Type", "Тип");
            dataGridView.Columns.Add("Category", "Категория");
            dataGridView.Columns.Add("Amount", "Сумма");
            dataGridView.Columns.Add("Date", "Дата");

            foreach (var income in data.Incomes)
            {
                dataGridView.Rows.Add("Доход", "", income.Amount, income.Date.ToShortDateString());
            }

            foreach (var expense in data.Expenses)
            {
                dataGridView.Rows.Add("Расход", expense.Category, expense.Amount, expense.Date.ToShortDateString());
            }
        }

        private void UpdateBalance()
        {
            decimal totalIncome = 0;
            decimal totalExpenses = 0;

            foreach (var i in data.Incomes) totalIncome += i.Amount;
            foreach (var e in data.Expenses) totalExpenses += e.Amount;

            labelBalance.Text = $"Остаток: {totalIncome - totalExpenses} руб.";
        }

        private void ClearInputs()
        {
            textBoxAmount.Text = "";
            textBoxCategory.Text = "";
            dateTimePicker.Value = DateTime.Now;
        }

        private Button buttonAddExpense;
        private DateTimePicker dateTimePicker1;
        private Label label1;
        private DataGridView dataGridView1;
    }

    public class Expense
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    public class Income
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    public class DataStorage
    {
        public List<Expense> Expenses { get; set; } = new List<Expense>();
        public List<Income> Incomes { get; set; } = new List<Income>();
    }
}
