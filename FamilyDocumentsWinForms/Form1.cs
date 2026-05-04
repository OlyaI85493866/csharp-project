using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MyLibrary;
using FamilyDocumentsWinForms.Services;

namespace FamilyDocumentsWinForms;

public partial class Form1 : Form
{
    private List<FamilyDocument> documents = new List<FamilyDocument>();
    private DocumentStorageService storageService = new DocumentStorageService();

    private Label labelHeader = null!;
    private Label labelId = null!;
    private Label labelTitle = null!;
    private Label labelType = null!;
    private Label labelDate = null!;
    private Label labelInfo = null!;

    private NumericUpDown numericId = null!;
    private TextBox textBoxTitle = null!;
    private ComboBox comboBoxType = null!;
    private DateTimePicker dateTimePickerDocument = null!;
    private CheckBox checkBoxImportant = null!;
    private Button buttonAdd = null!;
    private Button buttonClear = null!;
    private ListBox listBoxDocuments = null!;

    public Form1()
    {
        InitializeComponent();
        CreateFormElements();

        documents = storageService.LoadDocuments();
        RefreshDocumentsList();
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
        this.Controls.Add(buttonAdd);

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

    private void RefreshDocumentsList()
    {
        listBoxDocuments.Items.Clear();

        foreach (FamilyDocument document in documents)
        {
            string importantText = document.IsImportant ? "важный" : "обычный";

            string documentInfo =
                document.Id + " - " + document.Title +
                " | Тип: " + document.Category +
                " | Дата: " + document.DocumentDate.ToShortDateString() +
                " | Статус: " + importantText;

            listBoxDocuments.Items.Add(documentInfo);
        }
    }

    private void NumericId_ValueChanged(object? sender, EventArgs e)
    {
        labelInfo.Text = "Выбран Id документа: " + numericId.Value;
    }

    private void TextBoxTitle_TextChanged(object? sender, EventArgs e)
    {
        labelInfo.Text = "Вводится название документа: " + textBoxTitle.Text;
    }

    private void ComboBoxType_SelectedIndexChanged(object? sender, EventArgs e)
    {
        string selectedType = comboBoxType.SelectedItem?.ToString() ?? "Не указано";
        labelInfo.Text = "Выбран тип документа: " + selectedType;
    }

    private void DateTimePickerDocument_ValueChanged(object? sender, EventArgs e)
    {
        labelInfo.Text = "Выбрана дата документа: " + dateTimePickerDocument.Value.ToShortDateString();
    }

    private void CheckBoxImportant_CheckedChanged(object? sender, EventArgs e)
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

    private void ButtonAdd_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(textBoxTitle.Text))
        {
            labelInfo.Text = "Введите название документа.";
            return;
        }

        int id = (int)numericId.Value;
        string title = textBoxTitle.Text.Trim();
        string type = comboBoxType.SelectedItem?.ToString() ?? "Не указано";

        FamilyDocument document = new FamilyDocument(id, title)
        {
            Category = type,
            DocumentDate = dateTimePickerDocument.Value,
            IsImportant = checkBoxImportant.Checked
        };

        documents.Add(document);
        storageService.SaveDocuments(documents);
        RefreshDocumentsList();

        labelInfo.Text = "Документ добавлен и сохранен:\n" + document.GetInfo();
    }

    private void ButtonClear_Click(object? sender, EventArgs e)
    {
        numericId.Value = 1;
        textBoxTitle.Clear();
        comboBoxType.SelectedIndex = 0;
        dateTimePickerDocument.Value = DateTime.Today;
        checkBoxImportant.Checked = false;

        labelInfo.Text = "Поля очищены.";
    }

    private void ListBoxDocuments_SelectedIndexChanged(object? sender, EventArgs e)
    {
        int selectedIndex = listBoxDocuments.SelectedIndex;

        if (selectedIndex >= 0 && selectedIndex < documents.Count)
        {
            FamilyDocument selectedDocument = documents[selectedIndex];
            labelInfo.Text = "Выбран документ:\n" + selectedDocument.GetInfo();
        }
    }
}