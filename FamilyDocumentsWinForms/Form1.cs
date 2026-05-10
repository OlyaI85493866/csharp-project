using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MyLibrary;
using FamilyDocumentsWinForms.Services;
using Microsoft.VisualBasic;

namespace FamilyDocumentsWinForms;

public partial class Form1 : Form
{
    private string userRole;
    private List<FamilyDocument> documents = new List<FamilyDocument>();
    private List<FamilyDocument> filteredDocuments = new List<FamilyDocument>();
    private List<string> owners = new List<string>();

    private DocumentStorageService storageService = new DocumentStorageService();
    private OwnerStorageService ownerStorageService = new OwnerStorageService();

    private Label labelHeader = null!;
    private Label labelId = null!;
    private Label labelTitle = null!;
    private Label labelType = null!;
    private Label labelOwner = null!;
    private Label labelDocumentNumber = null!;
    private Label labelDate = null!;
    private Label labelExpirationDate = null!;
    private Label labelComment = null!;
    private Label labelFile = null!;
    private TextBox labelInfo = null!;
    private Label labelSearch = null!;
    private Label labelFilter = null!;
    private Label labelOwnerFilter = null!;
    private NumericUpDown numericId = null!;
    private TextBox textBoxTitle = null!;
    private ComboBox comboBoxType = null!;
    private ComboBox comboBoxOwner = null!;
    private ContextMenuStrip ownerContextMenu = null!;
    private TextBox textBoxDocumentNumber = null!;
    private DateTimePicker dateTimePickerDocument = null!;
    private DateTimePicker dateTimePickerExpiration = null!;
    private CheckBox checkBoxHasExpiration = null!;
    private TextBox textBoxComment = null!;
    private TextBox textBoxFilePath = null!;
    private TextBox textBoxSearch = null!;
    private ComboBox comboBoxFilter = null!;
    private ComboBox comboBoxOwnerFilter = null!;
    private CheckBox checkBoxImportant = null!;
    private CheckBox checkBoxOnlyImportant = null!;
    private Button buttonAdd = null!;
    private Button buttonClear = null!;
    private Button buttonEdit = null!;
    private Button buttonSelectFile = null!;
    private Button buttonOpenFile = null!;
    private Button buttonResetSearch = null!;
    private Button buttonDeleteAll = null!;
    private Button buttonAddOwner = null!;
    private Button buttonStatistics = null!;
    private DataGridView dataGridViewDocuments = null!;
    private ContextMenuStrip documentContextMenu = null!;

    public Form1(string role)
    {
        userRole = role;

        InitializeComponent();
        CreateFormElements();

        documents = storageService.LoadDocuments();
        owners = ownerStorageService.LoadOwners();

        RefreshOwnersList();

        filteredDocuments = new List<FamilyDocument>(documents);
        RefreshDocumentsList();
        SetNextDocumentId();

        ApplyUserRole();
    }

    private void CreateFormElements()
    {
        this.Text = "Семейная документация";
        this.Size = new Size(1250, 960);
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

        labelOwner = new Label();
        labelOwner.Text = "Владелец:";
        labelOwner.Location = new Point(30, 180);
        labelOwner.AutoSize = true;
        this.Controls.Add(labelOwner);

        comboBoxOwner = new ComboBox();
        comboBoxOwner.Location = new Point(230, 180);
        comboBoxOwner.Size = new Size(130, 25);
        comboBoxOwner.DropDownStyle = ComboBoxStyle.DropDownList;
        this.Controls.Add(comboBoxOwner);

        ownerContextMenu = new ContextMenuStrip();
        ToolStripMenuItem deleteOwnerMenuItem = new ToolStripMenuItem("Удалить владельца");
        deleteOwnerMenuItem.Click += DeleteOwnerMenuItem_Click;
        ownerContextMenu.Items.Add(deleteOwnerMenuItem);
        comboBoxOwner.ContextMenuStrip = ownerContextMenu;

        buttonAddOwner = new Button();
        buttonAddOwner.Text = "+";
        buttonAddOwner.Location = new Point(370, 180);
        buttonAddOwner.Size = new Size(60, 32);
        buttonAddOwner.BackColor = Color.FromArgb(52, 152, 219);
        buttonAddOwner.ForeColor = Color.White;
        buttonAddOwner.FlatStyle = FlatStyle.Flat;
        buttonAddOwner.FlatAppearance.BorderSize = 0;
        buttonAddOwner.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        buttonAddOwner.Cursor = Cursors.Hand;
        buttonAddOwner.Click += ButtonAddOwner_Click;
        this.Controls.Add(buttonAddOwner);

        labelDocumentNumber = new Label();
        labelDocumentNumber.Text = "Номер документа:";
        labelDocumentNumber.Location = new Point(30, 220);
        labelDocumentNumber.AutoSize = true;
        this.Controls.Add(labelDocumentNumber);

        textBoxDocumentNumber = new TextBox();
        textBoxDocumentNumber.Location = new Point(230, 220);
        textBoxDocumentNumber.Size = new Size(200, 25);
        this.Controls.Add(textBoxDocumentNumber);

        labelDate = new Label();
        labelDate.Text = "Дата документа:";
        labelDate.Location = new Point(30, 260);
        labelDate.AutoSize = true;
        this.Controls.Add(labelDate);

        dateTimePickerDocument = new DateTimePicker();
        dateTimePickerDocument.Location = new Point(230, 260);
        dateTimePickerDocument.Size = new Size(200, 25);
        dateTimePickerDocument.Format = DateTimePickerFormat.Short;
        dateTimePickerDocument.ValueChanged += DateTimePickerDocument_ValueChanged;
        this.Controls.Add(dateTimePickerDocument);

        labelExpirationDate = new Label();
        labelExpirationDate.Text = "Срок действия:";
        labelExpirationDate.Location = new Point(30, 300);
        labelExpirationDate.AutoSize = true;
        labelExpirationDate.Visible = false;
        this.Controls.Add(labelExpirationDate);

        dateTimePickerExpiration = new DateTimePicker();
        dateTimePickerExpiration.Location = new Point(230, 300);
        dateTimePickerExpiration.Size = new Size(200, 25);
        dateTimePickerExpiration.Format = DateTimePickerFormat.Short;
        dateTimePickerExpiration.Visible = false;
        this.Controls.Add(dateTimePickerExpiration);

        checkBoxHasExpiration = new CheckBox();
        checkBoxHasExpiration.Text = "Есть срок действия";
        checkBoxHasExpiration.Location = new Point(230, 330);
        checkBoxHasExpiration.AutoSize = true;
        checkBoxHasExpiration.CheckedChanged += CheckBoxHasExpiration_CheckedChanged;
        this.Controls.Add(checkBoxHasExpiration);

        labelComment = new Label();
        labelComment.Text = "Комментарий:";
        labelComment.Location = new Point(30, 365);
        labelComment.AutoSize = true;
        this.Controls.Add(labelComment);

        textBoxComment = new TextBox();
        textBoxComment.Location = new Point(230, 365);
        textBoxComment.Size = new Size(200, 60);
        textBoxComment.Multiline = true;
        this.Controls.Add(textBoxComment);

        labelFile = new Label();
        labelFile.Text = "Файл:";
        labelFile.Location = new Point(30, 435);
        labelFile.AutoSize = true;
        this.Controls.Add(labelFile);

        textBoxFilePath = new TextBox();
        textBoxFilePath.Location = new Point(230, 435);
        textBoxFilePath.Size = new Size(200, 25);
        textBoxFilePath.ReadOnly = true;
        this.Controls.Add(textBoxFilePath);

        buttonSelectFile = new Button();
        buttonSelectFile.Text = "Выбрать файл";
        buttonSelectFile.Location = new Point(230, 470);
        buttonSelectFile.Size = new Size(200, 35);
        buttonSelectFile.BackColor = Color.FromArgb(155, 89, 182);
        buttonSelectFile.ForeColor = Color.White;
        buttonSelectFile.FlatStyle = FlatStyle.Flat;
        buttonSelectFile.FlatAppearance.BorderSize = 0;
        buttonSelectFile.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        buttonSelectFile.Cursor = Cursors.Hand;
        buttonSelectFile.Click += ButtonSelectFile_Click;
        this.Controls.Add(buttonSelectFile);

        buttonOpenFile = new Button();
        buttonOpenFile.Text = "Открыть файл";
        buttonOpenFile.Location = new Point(30, 470);
        buttonOpenFile.Size = new Size(170, 35);
        buttonOpenFile.BackColor = Color.FromArgb(52, 73, 94);
        buttonOpenFile.ForeColor = Color.White;
        buttonOpenFile.FlatStyle = FlatStyle.Flat;
        buttonOpenFile.FlatAppearance.BorderSize = 0;
        buttonOpenFile.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        buttonOpenFile.Cursor = Cursors.Hand;
        buttonOpenFile.Click += ButtonOpenFile_Click;
        this.Controls.Add(buttonOpenFile);

        checkBoxImportant = new CheckBox();
        checkBoxImportant.Text = "Важный документ";
        checkBoxImportant.Location = new Point(230, 510);
        checkBoxImportant.AutoSize = true;
        checkBoxImportant.CheckedChanged += CheckBoxImportant_CheckedChanged;
        this.Controls.Add(checkBoxImportant);

        buttonAdd = new Button();
        buttonAdd.Text = "Добавить";
        buttonAdd.Location = new Point(30, 555);
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
        buttonClear.Location = new Point(230, 555);
        buttonClear.Size = new Size(170, 40);
        buttonClear.BackColor = Color.FromArgb(149, 165, 166);
        buttonClear.ForeColor = Color.White;
        buttonClear.FlatStyle = FlatStyle.Flat;
        buttonClear.FlatAppearance.BorderSize = 0;
        buttonClear.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        buttonClear.Cursor = Cursors.Hand;
        buttonClear.Click += ButtonClear_Click;
        this.Controls.Add(buttonClear);

        buttonDeleteAll = new Button();
        buttonDeleteAll.Text = "Удалить все";
        buttonDeleteAll.Location = new Point(30, 605);
        buttonDeleteAll.Size = new Size(170, 40);
        buttonDeleteAll.BackColor = Color.FromArgb(192, 57, 43);
        buttonDeleteAll.ForeColor = Color.White;
        buttonDeleteAll.FlatStyle = FlatStyle.Flat;
        buttonDeleteAll.FlatAppearance.BorderSize = 0;
        buttonDeleteAll.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        buttonDeleteAll.Cursor = Cursors.Hand;
        buttonDeleteAll.Click += ButtonDeleteAll_Click;
        this.Controls.Add(buttonDeleteAll);

        buttonStatistics = new Button();
        buttonStatistics.Text = "Статистика";
        buttonStatistics.Location = new Point(850, 430);
        buttonStatistics.Size = new Size(170, 40);
        buttonStatistics.BackColor = Color.FromArgb(41, 128, 185);
        buttonStatistics.ForeColor = Color.White;
        buttonStatistics.FlatStyle = FlatStyle.Flat;
        buttonStatistics.FlatAppearance.BorderSize = 0;
        buttonStatistics.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        buttonStatistics.Cursor = Cursors.Hand;
        buttonStatistics.Click += ButtonStatistics_Click;
        this.Controls.Add(buttonStatistics);

        buttonEdit = new Button();
        buttonEdit.Text = "Изменить";
        buttonEdit.Location = new Point(230, 605);
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
        textBoxSearch.Size = new Size(190, 25);
        textBoxSearch.TextChanged += SearchControls_Changed;
        this.Controls.Add(textBoxSearch);

        labelFilter = new Label();
        labelFilter.Text = "Тип:";
        labelFilter.Location = new Point(730, 55);
        labelFilter.AutoSize = true;
        this.Controls.Add(labelFilter);

        comboBoxFilter = new ComboBox();
        comboBoxFilter.Location = new Point(780, 52);
        comboBoxFilter.Size = new Size(160, 25);
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

        labelOwnerFilter = new Label();
        labelOwnerFilter.Text = "Владелец:";
        labelOwnerFilter.Location = new Point(945, 55);
        labelOwnerFilter.AutoSize = true;
        this.Controls.Add(labelOwnerFilter);

        comboBoxOwnerFilter = new ComboBox();
        comboBoxOwnerFilter.Location = new Point(1040, 52);
        comboBoxOwnerFilter.Size = new Size(170, 25);
        comboBoxOwnerFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBoxOwnerFilter.SelectedIndexChanged += SearchControls_Changed;
        this.Controls.Add(comboBoxOwnerFilter);

        checkBoxOnlyImportant = new CheckBox();
        checkBoxOnlyImportant.Text = "Только важные";
        checkBoxOnlyImportant.Location = new Point(460, 85);
        checkBoxOnlyImportant.AutoSize = true;
        checkBoxOnlyImportant.CheckedChanged += SearchControls_Changed;
        this.Controls.Add(checkBoxOnlyImportant);

        dataGridViewDocuments = new DataGridView();
        dataGridViewDocuments.Location = new Point(460, 120);
        dataGridViewDocuments.Size = new Size(750, 270);
        dataGridViewDocuments.ReadOnly = true;
        dataGridViewDocuments.AllowUserToAddRows = false;
        dataGridViewDocuments.AllowUserToDeleteRows = false;
        dataGridViewDocuments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridViewDocuments.MultiSelect = false;
        dataGridViewDocuments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        dataGridViewDocuments.RowHeadersVisible = false;
        dataGridViewDocuments.SelectionChanged += DataGridViewDocuments_SelectionChanged;
        dataGridViewDocuments.CellMouseDown += DataGridViewDocuments_CellMouseDown;

        dataGridViewDocuments.Columns.Add("Id", "ID");
        dataGridViewDocuments.Columns.Add("Title", "Название");
        dataGridViewDocuments.Columns.Add("Category", "Тип");
        dataGridViewDocuments.Columns.Add("Date", "Дата");
        dataGridViewDocuments.Columns.Add("Status", "Статус");
        dataGridViewDocuments.Columns.Add("ExpirationStatus", "Срок");

        dataGridViewDocuments.Columns["Id"].Width = 50;
        dataGridViewDocuments.Columns["Title"].Width = 230;
        dataGridViewDocuments.Columns["Category"].Width = 160;
        dataGridViewDocuments.Columns["Date"].Width = 100;
        dataGridViewDocuments.Columns["Status"].Width = 100;
        dataGridViewDocuments.Columns["ExpirationStatus"].Width = 110;
        this.Controls.Add(dataGridViewDocuments);
        
        documentContextMenu = new ContextMenuStrip();
        ToolStripMenuItem deleteDocumentMenuItem = new ToolStripMenuItem("Удалить документ");
        deleteDocumentMenuItem.Click += DeleteDocumentMenuItem_Click;
        ToolStripMenuItem openFileMenuItem = new ToolStripMenuItem("Открыть файл");
        openFileMenuItem.Click += OpenFileMenuItem_Click;
        ToolStripMenuItem copyFilePathMenuItem = new ToolStripMenuItem("Скопировать путь к файлу");
        copyFilePathMenuItem.Click += CopyFilePathMenuItem_Click;
        ToolStripMenuItem clearFileMenuItem = new ToolStripMenuItem("Убрать прикрепленный файл");
        clearFileMenuItem.Click += ClearFileMenuItem_Click;

        documentContextMenu.Items.Add(deleteDocumentMenuItem);
        documentContextMenu.Items.Add(openFileMenuItem);
        documentContextMenu.Items.Add(copyFilePathMenuItem);
        documentContextMenu.Items.Add(clearFileMenuItem);

        dataGridViewDocuments.ContextMenuStrip = documentContextMenu;

        buttonResetSearch = new Button();
        buttonResetSearch.Text = "Сбросить";
        buttonResetSearch.Location = new Point(1040, 430);
        buttonResetSearch.Size = new Size(170, 40);
        buttonResetSearch.BackColor = Color.FromArgb(149, 165, 166);
        buttonResetSearch.ForeColor = Color.White;
        buttonResetSearch.FlatStyle = FlatStyle.Flat;
        buttonResetSearch.FlatAppearance.BorderSize = 0;
        buttonResetSearch.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        buttonResetSearch.Cursor = Cursors.Hand;
        buttonResetSearch.Click += ButtonResetSearch_Click;
        this.Controls.Add(buttonResetSearch);

        labelInfo = new TextBox();
        labelInfo.Text = "Информация о документе появится здесь.";
        labelInfo.Location = new Point(30, 690);
        labelInfo.Size = new Size(1180, 170);
        labelInfo.BackColor = Color.White;
        labelInfo.ForeColor = Color.FromArgb(44, 62, 80);
        labelInfo.BorderStyle = BorderStyle.FixedSingle;
        labelInfo.Multiline = true;
        labelInfo.ReadOnly = true;
        labelInfo.ScrollBars = ScrollBars.Vertical;
        labelInfo.Font = new Font("Segoe UI", 10);
        this.Controls.Add(labelInfo);
    }

    private void RefreshDocumentsList()
    {
        dataGridViewDocuments.Rows.Clear();

        foreach (FamilyDocument document in filteredDocuments)
        {
            string importantText = document.IsImportant ? "важный" : "обычный";
            string expirationStatus = GetExpirationStatus(document);

            int rowIndex = dataGridViewDocuments.Rows.Add(
                document.Id,
                document.Title,
                document.Category,
                document.DocumentDate.ToShortDateString(),
                importantText,
                expirationStatus
            );

            dataGridViewDocuments.Rows[rowIndex].Tag = document;

            if (expirationStatus == "Истек")
            {
                dataGridViewDocuments.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220);
            }
            else if (expirationStatus == "Скоро истекает")
            {
                dataGridViewDocuments.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 200);
            }
            else if (document.IsImportant)
            {
                dataGridViewDocuments.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(220, 240, 255);
            }
        }
    }

    private int GetNextDocumentId()
    {
        if (documents.Count == 0)
        {
            return 1;
        }

        return documents.Max(document => document.Id) + 1;
    }
    private void SetNextDocumentId()
    {
        numericId.Value = GetNextDocumentId();
    }

    private void RefreshOwnersList()
    {
        comboBoxOwner.Items.Clear();
        comboBoxOwnerFilter.Items.Clear();

        comboBoxOwnerFilter.Items.Add("Все");

        foreach (string owner in owners)
        {
            comboBoxOwner.Items.Add(owner);
            comboBoxOwnerFilter.Items.Add(owner);
        }

        if (comboBoxOwner.Items.Count > 0)
        {
            comboBoxOwner.SelectedIndex = 0;
        }

        comboBoxOwnerFilter.SelectedIndex = 0;
    }

    private void ApplyFilters()
    {
        string searchText = textBoxSearch.Text.Trim().ToLower();
        string selectedCategory = comboBoxFilter.SelectedItem?.ToString() ?? "Все";
        string selectedOwner = comboBoxOwnerFilter.SelectedItem?.ToString() ?? "Все";
        bool onlyImportant = checkBoxOnlyImportant.Checked;

        filteredDocuments = documents
            .Where(document =>
                document.Title.ToLower().Contains(searchText) &&
                (selectedCategory == "Все" || document.Category == selectedCategory) &&
                (selectedOwner == "Все" || document.Owner == selectedOwner) &&
                (!onlyImportant || document.IsImportant)
            )
            .ToList();

        RefreshDocumentsList();
    }

    private string FormatDocumentInfo(FamilyDocument document)
    {
        string expirationDateText = document.ExpirationDate.HasValue
            ? document.ExpirationDate.Value.ToShortDateString()
            : "не указан";

        return
            "ID: " + document.Id + Environment.NewLine +
            "Название: " + document.Title + Environment.NewLine +
            "Категория: " + document.Category + Environment.NewLine +
            "Владелец: " + document.Owner + Environment.NewLine +
            "Номер документа: " + document.DocumentNumber + Environment.NewLine +
            "Дата документа: " + document.DocumentDate.ToShortDateString() + Environment.NewLine +
            "Срок действия: " + expirationDateText + Environment.NewLine +
            "Важный документ: " + (document.IsImportant ? "да" : "нет") + Environment.NewLine +
            "Файл: " + document.FilePath + Environment.NewLine +
            "Комментарий: " + document.Comment;
    }
    private string GetExpirationStatus(FamilyDocument document)
    {
        if (!document.ExpirationDate.HasValue)
        {
            return "Не указан";
        }

        DateTime today = DateTime.Today;
        DateTime expirationDate = document.ExpirationDate.Value.Date;

        if (expirationDate < today)
        {
            return "Истек";
        }

        int daysLeft = (expirationDate - today).Days;

        if (daysLeft <= 30)
        {
            return "Скоро истекает";
        }

        return "Действует";
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

    private void CheckBoxHasExpiration_CheckedChanged(object? sender, EventArgs e)
    {
        bool hasExpiration = checkBoxHasExpiration.Checked;

        labelExpirationDate.Visible = hasExpiration;
        dateTimePickerExpiration.Visible = hasExpiration;

        if (hasExpiration)
        {
            labelInfo.Text = "Для документа указан срок действия.";
        }
        else
        {
            labelInfo.Text = "Срок действия для документа не указан.";
        }
    }

    private void ButtonAdd_Click(object? sender, EventArgs e)
    {
        if (userRole == "viewer")
        {
            labelInfo.Text = "Недостаточно прав для добавления документа.";
            return;
        }
        if (string.IsNullOrWhiteSpace(textBoxTitle.Text))
        {
            labelInfo.Text = "Введите название документа.";
            return;
        }

        int id = (int)numericId.Value;
        string title = textBoxTitle.Text.Trim();
        string type = comboBoxType.SelectedItem?.ToString() ?? "Не указано";
        bool idExists = documents.Any(document => document.Id == id);
        if (idExists)
        {
            labelInfo.Text = "Документ с таким ID уже существует. Выберите другой ID.";
            return;
        }

        FamilyDocument document = new FamilyDocument(id, title)
        {
            Category = type,
            Owner = comboBoxOwner.SelectedItem?.ToString() ?? "",
            DocumentNumber = textBoxDocumentNumber.Text.Trim(),
            DocumentDate = dateTimePickerDocument.Value,
            ExpirationDate = checkBoxHasExpiration.Checked
                ? dateTimePickerExpiration.Value
                : null,
            FilePath = textBoxFilePath.Text.Trim(),
            Comment = textBoxComment.Text.Trim(),
            IsImportant = checkBoxImportant.Checked
        };

        documents.Add(document);
        storageService.SaveDocuments(documents);
        ApplyFilters();
        SetNextDocumentId();

        labelInfo.Text = "Документ добавлен и сохранен:" + Environment.NewLine + FormatDocumentInfo(document);
    }

    private void ButtonClear_Click(object? sender, EventArgs e)
    {
        numericId.Value = 1;
        textBoxTitle.Clear();
        comboBoxType.SelectedIndex = 0;
        if (comboBoxOwner.Items.Count > 0)
        {
            comboBoxOwner.SelectedIndex = 0;
        }
        textBoxDocumentNumber.Clear();
        dateTimePickerDocument.Value = DateTime.Today;
        checkBoxHasExpiration.Checked = false;
        dateTimePickerExpiration.Value = DateTime.Today;
        textBoxFilePath.Clear();
        textBoxComment.Clear();
        checkBoxImportant.Checked = false;

        labelInfo.Text = "Поля очищены.";
    }

    private void ButtonEdit_Click(object? sender, EventArgs e)
    {
        if (userRole == "viewer")
        {
            labelInfo.Text = "Недостаточно прав для изменения документа.";
            return;
        }
        if (dataGridViewDocuments.CurrentRow == null)
        {
            labelInfo.Text = "Выберите документ для изменения.";
            return;
        }

        if (dataGridViewDocuments.CurrentRow.Tag is not FamilyDocument selectedDocument)
        {
            labelInfo.Text = "Документ для изменения не найден.";
            return;
        }

        if (string.IsNullOrWhiteSpace(textBoxTitle.Text))
        {
            labelInfo.Text = "Введите название документа.";
            return;
        }

        selectedDocument.Id = (int)numericId.Value;
        selectedDocument.Title = textBoxTitle.Text.Trim();
        selectedDocument.Category = comboBoxType.SelectedItem?.ToString() ?? "Не указано";
        selectedDocument.Owner = comboBoxOwner.SelectedItem?.ToString() ?? "";
        selectedDocument.DocumentNumber = textBoxDocumentNumber.Text.Trim();
        selectedDocument.DocumentDate = dateTimePickerDocument.Value;
        selectedDocument.ExpirationDate = checkBoxHasExpiration.Checked
            ? dateTimePickerExpiration.Value
            : null;
        selectedDocument.FilePath = textBoxFilePath.Text.Trim();
        selectedDocument.Comment = textBoxComment.Text.Trim();
        selectedDocument.IsImportant = checkBoxImportant.Checked;

        storageService.SaveDocuments(documents);
        ApplyFilters();

        labelInfo.Text = "Документ изменен и сохранен:" + Environment.NewLine + FormatDocumentInfo(selectedDocument);
    }

    private void ButtonDelete_Click(object? sender, EventArgs e)
    {
        if (userRole == "viewer")
        {
            labelInfo.Text = "Недостаточно прав для удаления документа.";
            return;
        }
        if (dataGridViewDocuments.CurrentRow == null)
        {
            labelInfo.Text = "Выберите документ для удаления.";
            return;
        }

        if (dataGridViewDocuments.CurrentRow.Tag is not FamilyDocument selectedDocument)
        {
            labelInfo.Text = "Документ для удаления не найден.";
            return;
        }

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
        SetNextDocumentId();

        labelInfo.Text = "Документ удален:" + Environment.NewLine + FormatDocumentInfo(selectedDocument);
    }
    private void ButtonOpenFile_Click(object? sender, EventArgs e)
    {
        string filePath = textBoxFilePath.Text.Trim();

        if (string.IsNullOrWhiteSpace(filePath))
        {
            labelInfo.Text = "Файл для открытия не выбран.";
            return;
        }

        if (!File.Exists(filePath))
        {
            labelInfo.Text = "Файл не найден:" + Environment.NewLine + filePath;
            return;
        }

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = filePath,
            UseShellExecute = true
        };

        Process.Start(startInfo);
    }
    private void DataGridViewDocuments_SelectionChanged(object? sender, EventArgs e)
    {
        if (dataGridViewDocuments.CurrentRow == null)
        {
            return;
        }

        if (dataGridViewDocuments.CurrentRow.Tag is not FamilyDocument selectedDocument)
        {
            return;
        }

        numericId.Value = selectedDocument.Id;
        textBoxTitle.Text = selectedDocument.Title;

        int categoryIndex = comboBoxType.Items.IndexOf(selectedDocument.Category);
        if (categoryIndex >= 0)
        {
            comboBoxType.SelectedIndex = categoryIndex;
        }

        int ownerIndex = comboBoxOwner.Items.IndexOf(selectedDocument.Owner);
        if (ownerIndex >= 0)
        {
            comboBoxOwner.SelectedIndex = ownerIndex;
        }
        textBoxDocumentNumber.Text = selectedDocument.DocumentNumber;
        dateTimePickerDocument.Value = selectedDocument.DocumentDate;

        if (selectedDocument.ExpirationDate.HasValue)
        {
            checkBoxHasExpiration.Checked = true;
            dateTimePickerExpiration.Value = selectedDocument.ExpirationDate.Value;
        }
        else
        {
            checkBoxHasExpiration.Checked = false;
            dateTimePickerExpiration.Value = DateTime.Today;
        }

        textBoxFilePath.Text = selectedDocument.FilePath;
        textBoxComment.Text = selectedDocument.Comment;
        checkBoxImportant.Checked = selectedDocument.IsImportant;

        labelInfo.Text = "Выбран документ:" + Environment.NewLine + FormatDocumentInfo(selectedDocument);
    }

    private void SearchControls_Changed(object? sender, EventArgs e)
    {
        ApplyFilters();
    }

    private void ButtonResetSearch_Click(object? sender, EventArgs e)
    {
        textBoxSearch.Clear();
        comboBoxFilter.SelectedIndex = 0;
        comboBoxOwnerFilter.SelectedIndex = 0;
        checkBoxOnlyImportant.Checked = false;
        ApplyFilters();
    }
     private void ButtonSelectFile_Click(object? sender, EventArgs e)
    {
        if (userRole == "viewer")
        {
            labelInfo.Text = "Недостаточно прав для выбора файла.";
            return;
        }
        using OpenFileDialog openFileDialog = new OpenFileDialog();

        openFileDialog.Title = "Выберите файл документа";
        openFileDialog.Filter = "Документы и изображения|*.pdf;*.jpg;*.jpeg;*.png;*.docx;*.doc|Все файлы|*.*";

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            string copiedFilePath = CopyFileToStorage(openFileDialog.FileName);

            textBoxFilePath.Text = copiedFilePath;
            labelInfo.Text = "Файл выбран и скопирован в хранилище:" +
                            Environment.NewLine +
                            copiedFilePath;
        }
    }
    private string CopyFileToStorage(string sourceFilePath)
    {
        string storageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DocumentsStorage");

        if (!Directory.Exists(storageFolder))
        {
            Directory.CreateDirectory(storageFolder);
        }

        string originalFileName = Path.GetFileName(sourceFilePath);
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + originalFileName;
        string destinationFilePath = Path.Combine(storageFolder, uniqueFileName);

        File.Copy(sourceFilePath, destinationFilePath, true);

        return destinationFilePath;
    }
    private void ButtonDeleteAll_Click(object? sender, EventArgs e)
    {
        if (userRole == "viewer")
        {
            labelInfo.Text = "Недостаточно прав для удаления всех документов.";
            return;
        }
        if (documents.Count == 0)
        {
            labelInfo.Text = "Список документов уже пуст.";
            return;
        }

        DialogResult result = MessageBox.Show(
            "Вы действительно хотите удалить все документы?",
            "Подтверждение удаления",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );

        if (result != DialogResult.Yes)
        {
            return;
        }

        documents.Clear();
        filteredDocuments.Clear();

        storageService.SaveDocuments(documents);
        RefreshDocumentsList();

        textBoxSearch.Clear();
        comboBoxFilter.SelectedIndex = 0;

        numericId.Value = 1;
        textBoxTitle.Clear();
        comboBoxType.SelectedIndex = 0;

        if (comboBoxOwner.Items.Count > 0)
        {
            comboBoxOwner.SelectedIndex = 0;
        }

        textBoxDocumentNumber.Clear();
        dateTimePickerDocument.Value = DateTime.Today;
        checkBoxHasExpiration.Checked = false;
        dateTimePickerExpiration.Value = DateTime.Today;
        textBoxFilePath.Clear();
        textBoxComment.Clear();
        checkBoxImportant.Checked = false;

        labelInfo.Text = "Все документы удалены.";
    }
    private void ButtonAddOwner_Click(object? sender, EventArgs e)
    {
        if (userRole == "viewer")
        {
            labelInfo.Text = "Недостаточно прав для добавления владельца.";
            return;
        }
        string ownerName = Microsoft.VisualBasic.Interaction.InputBox(
            "Введите имя владельца:",
            "Добавление владельца",
            ""
        ).Trim();

        if (string.IsNullOrWhiteSpace(ownerName))
        {
            labelInfo.Text = "Имя владельца не введено.";
            return;
        }

        if (owners.Contains(ownerName))
        {
            labelInfo.Text = "Такой владелец уже есть в списке.";
            return;
        }

        owners.Add(ownerName);
        ownerStorageService.SaveOwners(owners);
        RefreshOwnersList();

        comboBoxOwner.SelectedItem = ownerName;
        labelInfo.Text = "Владелец добавлен: " + ownerName;
    }
    private void DeleteOwnerMenuItem_Click(object? sender, EventArgs e)
    {
        if (userRole == "viewer")
        {
            labelInfo.Text = "Недостаточно прав для удаления владельца.";
            return;
        }
        string ownerName = comboBoxOwner.SelectedItem?.ToString() ?? "";

        if (string.IsNullOrWhiteSpace(ownerName))
        {
            labelInfo.Text = "Выберите владельца для удаления.";
            return;
        }

        DialogResult result = MessageBox.Show(
            "Вы действительно хотите удалить владельца из списка?\n\n" + ownerName,
            "Подтверждение удаления владельца",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );

        if (result != DialogResult.Yes)
        {
            return;
        }

        owners.Remove(ownerName);
        ownerStorageService.SaveOwners(owners);
        RefreshOwnersList();
        ApplyFilters();

        labelInfo.Text = "Владелец удален из списка: " + ownerName;
    }

    private void DataGridViewDocuments_CellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
        {
            dataGridViewDocuments.ClearSelection();
            dataGridViewDocuments.Rows[e.RowIndex].Selected = true;
            dataGridViewDocuments.CurrentCell = dataGridViewDocuments.Rows[e.RowIndex].Cells[0];
        }
    }

    private void DeleteDocumentMenuItem_Click(object? sender, EventArgs e)
    {
        ButtonDelete_Click(sender, e);
    }

    private void OpenFileMenuItem_Click(object? sender, EventArgs e)
    {
        ButtonOpenFile_Click(sender, e);
    }

    private void CopyFilePathMenuItem_Click(object? sender, EventArgs e)
    {
        if (dataGridViewDocuments.CurrentRow == null)
        {
            labelInfo.Text = "Выберите документ.";
            return;
        }

        if (dataGridViewDocuments.CurrentRow.Tag is not FamilyDocument selectedDocument)
        {
            labelInfo.Text = "Документ не найден.";
            return;
        }

        if (string.IsNullOrWhiteSpace(selectedDocument.FilePath))
        {
            labelInfo.Text = "У выбранного документа нет прикрепленного файла.";
            return;
        }

        Clipboard.SetText(selectedDocument.FilePath);
        labelInfo.Text = "Путь к файлу скопирован:" + Environment.NewLine + selectedDocument.FilePath;
    }

    private void ClearFileMenuItem_Click(object? sender, EventArgs e)
    {
        if (userRole == "viewer")
        {
            labelInfo.Text = "Недостаточно прав для изменения файла документа.";
            return;
        }
        if (dataGridViewDocuments.CurrentRow == null)
        {
            labelInfo.Text = "Выберите документ.";
            return;
        }

        if (dataGridViewDocuments.CurrentRow.Tag is not FamilyDocument selectedDocument)
        {
            labelInfo.Text = "Документ не найден.";
            return;
        }

        if (string.IsNullOrWhiteSpace(selectedDocument.FilePath))
        {
            labelInfo.Text = "У выбранного документа нет прикрепленного файла.";
            return;
        }

        DialogResult result = MessageBox.Show(
            "Убрать прикрепленный файл у выбранного документа?",
            "Подтверждение",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        );

        if (result != DialogResult.Yes)
        {
            return;
        }

        selectedDocument.FilePath = "";
        textBoxFilePath.Clear();

        storageService.SaveDocuments(documents);
        ApplyFilters();

        labelInfo.Text = "Прикрепленный файл убран у документа:" + Environment.NewLine + selectedDocument.Title;
    }

    private void ButtonStatistics_Click(object? sender, EventArgs e)
    {
        int totalDocuments = documents.Count;
        int importantDocuments = documents.Count(document => document.IsImportant);
        int documentsWithExpiration = documents.Count(document => document.ExpirationDate.HasValue);
        int expiredDocuments = documents.Count(document => GetExpirationStatus(document) == "Истек");
        int expiringSoonDocuments = documents.Count(document => GetExpirationStatus(document) == "Скоро истекает");

        labelInfo.Text =
            "Статистика документов:" + Environment.NewLine +
            Environment.NewLine +
            "Всего документов: " + totalDocuments + Environment.NewLine +
            "Важных документов: " + importantDocuments + Environment.NewLine +
            "Документов со сроком действия: " + documentsWithExpiration + Environment.NewLine +
            "Документов с истекшим сроком: " + expiredDocuments + Environment.NewLine +
            "Документов, срок которых скоро истекает: " + expiringSoonDocuments;
    }
    private void ApplyUserRole()
    {
        if (userRole == "viewer")
        {
            buttonAdd.Enabled = false;
            buttonClear.Enabled = false;
            buttonEdit.Enabled = false;
            buttonDeleteAll.Enabled = false;
            buttonSelectFile.Enabled = false;
            buttonAddOwner.Enabled = false;

            numericId.Enabled = false;
            textBoxTitle.ReadOnly = true;
            comboBoxType.Enabled = false;
            comboBoxOwner.Enabled = false;
            textBoxDocumentNumber.ReadOnly = true;
            dateTimePickerDocument.Enabled = false;
            checkBoxHasExpiration.Enabled = false;
            dateTimePickerExpiration.Enabled = false;
            textBoxFilePath.ReadOnly = true;
            textBoxComment.ReadOnly = true;
            checkBoxImportant.Enabled = false;

            labelInfo.Text = "Вход выполнен в режиме просмотра. Изменение данных недоступно.";
        }
        else
        {
            labelInfo.Text = "Вход выполнен в режиме администратора.";
        }
    }
}