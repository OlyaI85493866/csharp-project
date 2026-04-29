using System;
using System.Drawing;
using System.Windows.Forms;
using MyLibrary;

namespace FamilyDocumentsWinForms;

public partial class Form1 : Form
{
    private Label labelHeader;
    private Label labelId;
    private Label labelTitle;
    private Label labelType;
    private Label labelDate;
    private Label labelInfo;

    private NumericUpDown numericId;
    private TextBox textBoxTitle;
    private ComboBox comboBoxType;
    private DateTimePicker dateTimePickerDocument;
    private CheckBox checkBoxImportant;
    private Button buttonAdd;
    private Button buttonClear;
    private ListBox listBoxDocuments;
    public Form1()
    {
        InitializeComponent();
        CreateFormElements();
    }

    private void CreateFormElements()
    {
        this.Text = "Семейная документация";
        this.Size = new Size(700, 540);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(245, 247, 250);
        this.Font = new Font("Segoe UI", 9);
        
        labelHeader = new Label();
        labelHeader.Text = "Семейная документация";
        labelHeader.Location = new Point(30, 10);
        labelHeader.AutoSize = true;
        labelHeader.Font = new Font("Segoe UI", 16, FontStyle.Bold);
        labelHeader.ForeColor = Color.FromArgb(44, 62, 80);
        this.Controls.Add(labelHeader);
                
        labelId = new Label();
        labelId.Text = "Id документа:";
        labelId.Location = new Point(30, 60);
        labelId.AutoSize = true;
        this.Controls.Add(labelId);
        
        numericId = new NumericUpDown();
        numericId.Location = new Point(180, 60);
        numericId.Size = new Size(200, 25);
        numericId.Minimum = 1;
        numericId.Maximum = 10000;
        numericId.BackColor = Color.White;
        numericId.ForeColor = Color.FromArgb(44, 62, 80);
        numericId.ValueChanged += NumericId_ValueChanged;
        this.Controls.Add(numericId);
        
        labelTitle = new Label();
        labelTitle.Text = "Название документа:";
        labelTitle.Location = new Point(30, 100);
        labelTitle.AutoSize = true;
        this.Controls.Add(labelTitle);
        
        textBoxTitle = new TextBox();
        textBoxTitle.Location = new Point(230, 100);
        textBoxTitle.Size = new Size(200, 25);
        textBoxTitle.TextChanged += TextBoxTitle_TextChanged;
        this.Controls.Add(textBoxTitle);

        labelType = new Label();
        labelType.Text = "Тип документа:";
        labelType.Location = new Point(30, 140);
        labelType.AutoSize = true;
        this.Controls.Add(labelType);

        comboBoxType = new ComboBox();
        comboBoxType.Location = new Point(230, 140);
        comboBoxType.Size = new Size(200, 25);
        comboBoxType.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBoxType.Items.Add("Паспорт");
        comboBoxType.Items.Add("Свидетельство");
        comboBoxType.Items.Add("Медицинский документ");
        comboBoxType.Items.Add("Договор");
        comboBoxType.Items.Add("Квитанция");
        comboBoxType.SelectedIndex = 0;
        comboBoxType.SelectedIndexChanged += ComboBoxType_SelectedIndexChanged;
        this.Controls.Add(comboBoxType);

        labelDate = new Label();
        labelDate.Text = "Дата документа:";
        labelDate.Location = new Point(30, 180);
        labelDate.AutoSize = true;
        this.Controls.Add(labelDate);

        dateTimePickerDocument = new DateTimePicker();
        dateTimePickerDocument.Location = new Point(230, 180);
        dateTimePickerDocument.Size = new Size(200, 25);
        dateTimePickerDocument.Format = DateTimePickerFormat.Short;
        dateTimePickerDocument.ValueChanged += DateTimePickerDocument_ValueChanged;
        this.Controls.Add(dateTimePickerDocument);

        checkBoxImportant = new CheckBox();
        checkBoxImportant.Text = "Важный документ";
        checkBoxImportant.Location = new Point(230, 220);
        checkBoxImportant.AutoSize = true;
        checkBoxImportant.CheckedChanged += CheckBoxImportant_CheckedChanged;
        this.Controls.Add(checkBoxImportant);

        buttonAdd = new Button();
        buttonAdd.Text = "Добавить документ";
        buttonAdd.Location = new Point(30, 270);
        buttonAdd.Size = new Size(170, 40);
        buttonAdd.BackColor = Color.FromArgb(52, 152, 219);
        buttonAdd.ForeColor = Color.White;
        buttonAdd.FlatStyle = FlatStyle.Flat;
        buttonAdd.FlatAppearance.BorderSize = 0;
        buttonAdd.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        buttonAdd.Cursor = Cursors.Hand;
        buttonAdd.Click += ButtonAdd_Click;
        this.Controls.Add(buttonAdd);buttonAdd = new Button();

        buttonClear = new Button();
        buttonClear.Text = "Очистить";
        buttonClear.Location = new Point(230, 270);
        buttonClear.Size = new Size(170, 40);
        buttonClear.BackColor = Color.FromArgb(149, 165, 166);
        buttonClear.ForeColor = Color.White;
        buttonClear.FlatStyle = FlatStyle.Flat;
        buttonClear.FlatAppearance.BorderSize = 0;
        buttonClear.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        buttonClear.Cursor = Cursors.Hand;
        buttonClear.Click += ButtonClear_Click;
        this.Controls.Add(buttonClear);


        listBoxDocuments = new ListBox();
        listBoxDocuments.Location = new Point(460, 30);
        listBoxDocuments.Size = new Size(200, 235);
        listBoxDocuments.SelectedIndexChanged += ListBoxDocuments_SelectedIndexChanged;
        this.Controls.Add(listBoxDocuments);

        labelInfo = new Label();
        labelInfo.Text = "Информация о документе появится здесь.";
        labelInfo.Location = new Point(30, 330);
        labelInfo.Size = new Size(630, 120);
        labelInfo.BackColor = Color.White;
        labelInfo.ForeColor = Color.FromArgb(44, 62, 80);
        labelInfo.BorderStyle = BorderStyle.FixedSingle;
        labelInfo.Padding = new Padding(10);
        labelInfo.Font = new Font("Segoe UI", 10);
        this.Controls.Add(labelInfo);
    }
    private void NumericId_ValueChanged(object sender, EventArgs e)
    {
        labelInfo.Text = "Выбран Id документа: " + numericId.Value;
    }
    private void TextBoxTitle_TextChanged(object sender, EventArgs e)
    {
        labelInfo.Text = "Вводится название документа: " + textBoxTitle.Text;
    }
    private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
    {
        labelInfo.Text = "Выбран тип документа: " + comboBoxType.SelectedItem.ToString();   
    }
    private void DateTimePickerDocument_ValueChanged(object sender, EventArgs e)
    {
        labelInfo.Text = "Выбрана дата документа: " + dateTimePickerDocument.Value.ToShortDateString();
    }
    private void CheckBoxImportant_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBoxImportant.Checked)
        {
            labelInfo.Text = "Документ отмечен как важный.";
        }
        else
        {
            labelInfo.Text = "Документ не отмечен как важный.";
        }
    }
    private void ButtonAdd_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(textBoxTitle.Text))
        {
            labelInfo.Text = "Введите название документа.";
            return;
        }

        int id = (int)numericId.Value;
        string title = textBoxTitle.Text.Trim();
        string type = comboBoxType.SelectedItem.ToString();
        string date = dateTimePickerDocument.Value.ToShortDateString();
        string importance = checkBoxImportant.Checked ? "важный" : "обычный";

        ExtendedDocument document = new ExtendedDocument(id, title, "Изместьева Ольга");
        document.CapitalizeTitle();

        string documentInfo = document.Id + " - " + document.Title +
                            " | Тип: " + type +
                            " | Дата: " + date +
                            " | Статус: " + importance;

        listBoxDocuments.Items.Add(documentInfo);

        labelInfo.Text = "Документ добавлен:\n" + documentInfo;
    }
    private void ButtonClear_Click(object sender, EventArgs e)
    {
        numericId.Value = 1;
        textBoxTitle.Clear();
        comboBoxType.SelectedIndex = 0;
        dateTimePickerDocument.Value = DateTime.Today;
        checkBoxImportant.Checked = false;

        labelInfo.Text = "Поля очищены.";
    }
        private void ListBoxDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listBoxDocuments.SelectedItem != null)
        {
            labelInfo.Text = "Выбран документ:\n" + listBoxDocuments.SelectedItem.ToString();
        }
    }
}

