using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MyLibrary;
using FamilyDocumentsWinForms.Services;

namespace FamilyDocumentsWinForms;

public partial class Form1 : Form
{
    private List<FamilyDocument> documents = new List<FamilyDocument>();
    private List<FamilyDocument> filteredDocuments = new List<FamilyDocument>();
    private DocumentStorageService storageService = new DocumentStorageService();

    private Label labelHeader = null!;
    private Label labelId = null!;
    private Label labelTitle = null!;
    private Label labelType = null!;
    private Label labelDate = null!;
    private Label labelInfo = null!;
    private Label labelSearch = null!;
    private Label labelFilter = null!;

    private NumericUpDown numericId = null!;
    private TextBox textBoxTitle = null!;
    private ComboBox comboBoxType = null!;
    private TextBox textBoxSearch = null!;
    private ComboBox comboBoxFilter = null!;
    private DateTimePicker dateTimePickerDocument = null!;
    private CheckBox checkBoxImportant = null!;

    private Button buttonAdd = null!;
    private Button buttonClear = null!;
    private Button buttonDelete = null!;
    private Button buttonEdit = null!;
    private Button buttonResetSearch = null!;

    private DataGridView dataGridViewDocuments = null!;

    public Form1()
    {
        InitializeComponent();
        CreateFormElements();

        documents = storageService.LoadDocuments();
        filteredDocuments = new List<FamilyDocument>(documents);
        RefreshDocumentsList();
    }

    private void CreateFormElements()
    {
        this.Text = "Семейная документация";
        this.Size = new Size(1100, 660);
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
        buttonAdd.Text = "Добавить";
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

        buttonDelete = new Button();
        buttonDelete.Text = "Удалить";
        buttonDelete.Location = new Point(30, 320);
        buttonDelete.Size = new Size(170, 40);
        buttonDelete.BackColor = Color.FromArgb(231, 76, 60);
        buttonDelete.ForeColor = Color.White;
        buttonDelete.FlatStyle = FlatStyle.Flat;
        buttonDelete.FlatAppearance.BorderSize = 0;
        buttonDelete.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        buttonDelete.Cursor = Cursors.Hand;
        buttonDelete.Click += ButtonDelete_Click;
        this.Controls.Add(buttonDelete);

        buttonEdit = new Button();
        buttonEdit.Text = "Изменить";
        buttonEdit.Location = new Point(230, 320);
        buttonEdit.Size = new Size(170, 40);
        buttonEdit.BackColor = Color.FromArgb(46, 204, 113);
        buttonEdit.ForeColor = Color.White;
        buttonEdit.FlatStyle = FlatStyle.Flat;
        buttonEdit.FlatAppearance.BorderSize = 0;
        buttonEdit.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        buttonEdit.Cursor = Cursors.Hand;
        buttonEdit.Click += ButtonEdit_Click;
        this.Controls.Add(buttonEdit);

        labelSearch = new Label();
        labelSearch.Text = "Поиск:";
        labelSearch.Location = new Point(460, 55);
        labelSearch.AutoSize = true;
        this.Controls.Add(labelSearch);

        textBoxSearch = new TextBox();
        textBoxSearch.Location = new Point(530, 52);
        textBoxSearch.Size = new Size(180, 25);
        textBoxSearch.TextChanged += SearchControls_Changed;
        this.Controls.Add(textBoxSearch);

        labelFilter = new Label();
        labelFilter.Text = "Тип:";
        labelFilter.Location = new Point(740, 55);
        labelFilter.AutoSize = true;
        this.Controls.Add(labelFilter);

        comboBoxFilter = new ComboBox();
        comboBoxFilter.Location = new Point(790, 52);
        comboBoxFilter.Size = new Size(260, 25);
        comboBoxFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBoxFilter.Items.Add("Все");
        comboBoxFilter.Items.Add("Паспорт");
        comboBoxFilter.Items.Add("Свидетельство");
        comboBoxFilter.Items.Add("Медицинский документ");
        comboBoxFilter.Items.Add("Договор");
        comboBoxFilter.Items.Add("Квитанция");
        comboBoxFilter.SelectedIndex = 0;
        comboBoxFilter.SelectedIndexChanged += SearchControls_Changed;
        this.Controls.Add(comboBoxFilter);

        dataGridViewDocuments = new DataGridView();
        dataGridViewDocuments.Location = new Point(460, 100);
        dataGridViewDocuments.Size = new Size(590, 270);
        dataGridViewDocuments.ReadOnly = true;
        dataGridViewDocuments.AllowUserToAddRows = false;
        dataGridViewDocuments.AllowUserToDeleteRows = false;
        dataGridViewDocuments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridViewDocuments.MultiSelect = false;
        dataGridViewDocuments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        dataGridViewDocuments.RowHeadersVisible = false;
        dataGridViewDocuments.SelectionChanged += DataGridViewDocuments_SelectionChanged;

        dataGridViewDocuments.Columns.Add("Id", "ID");
        dataGridViewDocuments.Columns.Add("Title", "Название");
        dataGridViewDocuments.Columns.Add("Category", "Тип");
        dataGridViewDocuments.Columns.Add("Date", "Дата");
        dataGridViewDocuments.Columns.Add("Status", "Статус");

        dataGridViewDocuments.Columns["Id"].Width = 50;
        dataGridViewDocuments.Columns["Title"].Width = 190;
        dataGridViewDocuments.Columns["Category"].Width = 160;
        dataGridViewDocuments.Columns["Date"].Width = 100;
        dataGridViewDocuments.Columns["Status"].Width = 90;

        this.Controls.Add(dataGridViewDocuments);

        buttonResetSearch = new Button();
        buttonResetSearch.Text = "Сбросить";
        buttonResetSearch.Location = new Point(880, 390);
        buttonResetSearch.Size = new Size(170, 40);
        buttonResetSearch.BackColor = Color.FromArgb(149, 165, 166);
        buttonResetSearch.ForeColor = Color.White;
        buttonResetSearch.FlatStyle = FlatStyle.Flat;
        buttonResetSearch.FlatAppearance.BorderSize = 0;
        buttonResetSearch.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        buttonResetSearch.Cursor = Cursors.Hand;
        buttonResetSearch.Click += ButtonResetSearch_Click;
        this.Controls.Add(buttonResetSearch);

        labelInfo = new Label();
        labelInfo.Text = "Информация о документе появится здесь.";
        labelInfo.Location = new Point(30, 450);
        labelInfo.Size = new Size(1020, 140);
        labelInfo.BackColor = Color.White;
        labelInfo.ForeColor = Color.FromArgb(44, 62, 80);
        labelInfo.BorderStyle = BorderStyle.FixedSingle;
        labelInfo.Padding = new Padding(10);
        labelInfo.Font = new Font("Segoe UI", 10);
        this.Controls.Add(labelInfo);
    }

    private void RefreshDocumentsList()
    {
        dataGridViewDocuments.Rows.Clear();

        foreach (FamilyDocument document in filteredDocuments)
        {
            string importantText = document.IsImportant ? "важный" : "обычный";

            dataGridViewDocuments.Rows.Add(
                document.Id,
                document.Title,
                document.Category,
                document.DocumentDate.ToShortDateString(),
                importantText
            );
        }
    }

    private void ApplyFilters()
    {
        string searchText = textBoxSearch.Text.Trim().ToLower();
        string selectedCategory = comboBoxFilter.SelectedItem?.ToString() ?? "Все";

        filteredDocuments = documents
            .Where(document =>
                document.Title.ToLower().Contains(searchText) &&
                (selectedCategory == "Все" || document.Category == selectedCategory)
            )
            .ToList();

        RefreshDocumentsList();
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
        ApplyFilters();

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

    private void ButtonDelete_Click(object? sender, EventArgs e)
    {
        if (dataGridViewDocuments.CurrentRow == null)
        {
            labelInfo.Text = "Выберите документ для удаления.";
            return;
        }

        int selectedIndex = dataGridViewDocuments.CurrentRow.Index;

        if (selectedIndex < 0 || selectedIndex >= filteredDocuments.Count)
        {
            labelInfo.Text = "Документ для удаления не найден.";
            return;
        }

        FamilyDocument selectedDocument = filteredDocuments[selectedIndex];

        DialogResult result = MessageBox.Show(
            "Вы действительно хотите удалить документ?\n\n" + selectedDocument.Title,
            "Подтверждение удаления",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );

        if (result != DialogResult.Yes)
        {
            return;
        }

        documents.Remove(selectedDocument);
        storageService.SaveDocuments(documents);
        ApplyFilters();

        labelInfo.Text = "Документ удален:\n" + selectedDocument.GetInfo();
    }

    private void DataGridViewDocuments_SelectionChanged(object? sender, EventArgs e)
    {
        if (dataGridViewDocuments.CurrentRow == null)
        {
            return;
        }

        int selectedIndex = dataGridViewDocuments.CurrentRow.Index;

        if (selectedIndex >= 0 && selectedIndex < filteredDocuments.Count)
        {
            FamilyDocument selectedDocument = filteredDocuments[selectedIndex];

            numericId.Value = selectedDocument.Id;
            textBoxTitle.Text = selectedDocument.Title;

            int categoryIndex = comboBoxType.Items.IndexOf(selectedDocument.Category);
            if (categoryIndex >= 0)
            {
                comboBoxType.SelectedIndex = categoryIndex;
            }

            dateTimePickerDocument.Value = selectedDocument.DocumentDate;
            checkBoxImportant.Checked = selectedDocument.IsImportant;

            labelInfo.Text = "Выбран документ:\n" + selectedDocument.GetInfo();
        }
    }
    private void ButtonEdit_Click(object? sender, EventArgs e)
    {
        if (dataGridViewDocuments.CurrentRow == null)
        {
            labelInfo.Text = "Выберите документ для изменения.";
            return;
        }

        int selectedIndex = dataGridViewDocuments.CurrentRow.Index;

        if (selectedIndex < 0 || selectedIndex >= filteredDocuments.Count)
        {
            labelInfo.Text = "Документ для изменения не найден.";
            return;
        }

        if (string.IsNullOrWhiteSpace(textBoxTitle.Text))
        {
            labelInfo.Text = "Введите название документа.";
            return;
        }

        FamilyDocument selectedDocument = filteredDocuments[selectedIndex];

        selectedDocument.Id = (int)numericId.Value;
        selectedDocument.Title = textBoxTitle.Text.Trim();
        selectedDocument.Category = comboBoxType.SelectedItem?.ToString() ?? "Не указано";
        selectedDocument.DocumentDate = dateTimePickerDocument.Value;
        selectedDocument.IsImportant = checkBoxImportant.Checked;

        storageService.SaveDocuments(documents);
        ApplyFilters();

        labelInfo.Text = "Документ изменен и сохранен:\n" + selectedDocument.GetInfo();
    }

    private void SearchControls_Changed(object? sender, EventArgs e)
    {
        ApplyFilters();
    }

    private void ButtonResetSearch_Click(object? sender, EventArgs e)
    {
        textBoxSearch.Clear();
        comboBoxFilter.SelectedIndex = 0;
        ApplyFilters();
    }
}