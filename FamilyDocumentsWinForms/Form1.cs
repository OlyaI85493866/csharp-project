using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MyLibrary;
using FamilyDocumentsWinForms.Services;
using Microsoft.VisualBasic;
using FamilyDocumentsWinForms.Controls;

namespace FamilyDocumentsWinForms;

public partial class Form1 : Form
{
    private string userRole;

    private List<FamilyDocument> documents = new List<FamilyDocument>();
    private List<FamilyDocument> filteredDocuments = new List<FamilyDocument>();
    private List<string> owners = new List<string>();
    private List<string> documentTypes = new List<string>();
    private bool isUpdatingControls = false;

    private DocumentStorageService storageService = new DocumentStorageService();
    private OwnerStorageService ownerStorageService = new OwnerStorageService();
    private DocumentTypeStorageService documentTypeStorageService = new DocumentTypeStorageService();

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

    private RoundedButton buttonAdd = null!;
    private RoundedButton buttonClear = null!;
    private RoundedButton buttonEdit = null!;
    private RoundedButton buttonSelectFile = null!;
    private RoundedButton buttonOpenFile = null!;
    private RoundedButton buttonResetSearch = null!;
    private RoundedButton buttonDeleteAll = null!;
    private RoundedButton buttonAddOwner = null!;
    private RoundedButton buttonAddType = null!;
    private RoundedButton buttonStatistics = null!;

    private DataGridView dataGridViewDocuments = null!;
    private ContextMenuStrip documentContextMenu = null!;
    private ToolTip toolTip = null!;

    private RoundedPanel panelDocument = null!;
    private RoundedPanel panelDocuments = null!;
    private RoundedPanel panelInfo = null!;

    private Panel shadowDocument = null!;
    private Panel shadowDocuments = null!;
    private Panel shadowInfo = null!;

    private Label labelDocumentPanelTitle = null!;
    private Label labelDocumentsPanelTitle = null!;
    private Label labelInfoPanelTitle = null!;

    public Form1(string role)
    {
        userRole = role;

        InitializeComponent();
        CreateFormElements();

        documents = storageService.LoadDocuments();
        owners = ownerStorageService.LoadOwners();
        documentTypes = documentTypeStorageService.LoadTypes();

        RefreshDocumentTypesList();
        RefreshOwnersList();

        filteredDocuments = new List<FamilyDocument>(documents);
        RefreshDocumentsList();
        SetNextDocumentId();

        ApplyUserRole();
    }

    private void CreateFormElements()
    {
        this.Text = "Семейная документация";
        this.Size = new Size(1260, 980);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(245, 247, 250);
        this.Font = new Font("Segoe UI", 9);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;

        toolTip = new ToolTip();
        toolTip.AutoPopDelay = 5000;
        toolTip.InitialDelay = 400;
        toolTip.ReshowDelay = 200;
        toolTip.ShowAlways = true;

        labelHeader = new Label();
        labelHeader.Text = "Семейная документация";
        labelHeader.Location = new Point(30, 10);
        labelHeader.AutoSize = true;
        labelHeader.Font = new Font("Segoe UI", 18, FontStyle.Bold);
        labelHeader.ForeColor = Color.FromArgb(44, 62, 80);
        this.Controls.Add(labelHeader);

        shadowDocument = CreateShadowPanel(new Point(20, 75), new Size(420, 650));
        this.Controls.Add(shadowDocument);

        panelDocument = new RoundedPanel();
        panelDocument.Location = new Point(20, 75);
        panelDocument.Size = new Size(420, 650);
        panelDocument.CornerRadius = 18;
        panelDocument.BorderColor = Color.FromArgb(225, 230, 235);
        this.Controls.Add(panelDocument);
        panelDocument.BringToFront();

        shadowDocuments = CreateShadowPanel(new Point(455, 75), new Size(770, 650));
        this.Controls.Add(shadowDocuments);

        panelDocuments = new RoundedPanel();
        panelDocuments.Location = new Point(455, 75);
        panelDocuments.Size = new Size(770, 650);
        panelDocuments.CornerRadius = 18;
        panelDocuments.BorderColor = Color.FromArgb(225, 230, 235);
        this.Controls.Add(panelDocuments);
        panelDocuments.BringToFront();

        shadowInfo = CreateShadowPanel(new Point(20, 735), new Size(1205, 170));
        this.Controls.Add(shadowInfo);

        panelInfo = new RoundedPanel();
        panelInfo.Location = new Point(20, 735);
        panelInfo.Size = new Size(1205, 170);
        panelInfo.CornerRadius = 18;
        panelInfo.BorderColor = Color.FromArgb(225, 230, 235);
        this.Controls.Add(panelInfo);
        panelInfo.BringToFront();

        labelDocumentPanelTitle = new Label();
        labelDocumentPanelTitle.Text = "Документ";
        labelDocumentPanelTitle.Location = new Point(20, 10);
        labelDocumentPanelTitle.AutoSize = true;
        labelDocumentPanelTitle.Font = new Font("Segoe UI", 13, FontStyle.Bold);
        labelDocumentPanelTitle.ForeColor = Color.FromArgb(44, 62, 80);
        panelDocument.Controls.Add(labelDocumentPanelTitle);

        labelDocumentsPanelTitle = new Label();
        labelDocumentsPanelTitle.Text = "Документы";
        labelDocumentsPanelTitle.Location = new Point(10, 10);
        labelDocumentsPanelTitle.AutoSize = true;
        labelDocumentsPanelTitle.Font = new Font("Segoe UI", 13, FontStyle.Bold);
        labelDocumentsPanelTitle.ForeColor = Color.FromArgb(44, 62, 80);
        panelDocuments.Controls.Add(labelDocumentsPanelTitle);

        labelInfoPanelTitle = new Label();
        labelInfoPanelTitle.Text = "Информация о документе";
        labelInfoPanelTitle.Location = new Point(20, 15);
        labelInfoPanelTitle.AutoSize = true;
        labelInfoPanelTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        labelInfoPanelTitle.ForeColor = Color.FromArgb(44, 62, 80);
        panelInfo.Controls.Add(labelInfoPanelTitle);

        labelId = new Label();
        labelId.Text = "Id документа:";
        labelId.Location = new Point(20, 55);
        labelId.AutoSize = true;
        StyleLabel(labelId);
        panelDocument.Controls.Add(labelId);

        numericId = new NumericUpDown();
        numericId.Location = new Point(210, 52);
        numericId.Size = new Size(170, 25);
        numericId.Minimum = 1;
        numericId.Maximum = 1000000;
        numericId.BackColor = Color.White;
        numericId.ForeColor = Color.FromArgb(44, 62, 80);
        numericId.ValueChanged += NumericId_ValueChanged;
        panelDocument.Controls.Add(numericId);

        labelTitle = new Label();
        labelTitle.Text = "Название документа:";
        labelTitle.Location = new Point(20, 95);
        labelTitle.AutoSize = true;
        StyleLabel(labelTitle);
        panelDocument.Controls.Add(labelTitle);

        textBoxTitle = new TextBox();
        textBoxTitle.Location = new Point(210, 92);
        textBoxTitle.Size = new Size(170, 25);
        StyleTextBox(textBoxTitle);
        textBoxTitle.TextChanged += TextBoxTitle_TextChanged;
        panelDocument.Controls.Add(textBoxTitle);

        labelType = new Label();
        labelType.Text = "Тип документа:";
        labelType.Location = new Point(20, 135);
        labelType.AutoSize = true;
        StyleLabel(labelType);
        panelDocument.Controls.Add(labelType);

        comboBoxType = new ComboBox();
        comboBoxType.Location = new Point(210, 132);
        comboBoxType.Size = new Size(125, 25);
        comboBoxType.DropDownStyle = ComboBoxStyle.DropDownList;
        StyleComboBox(comboBoxType);
        comboBoxType.SelectedIndexChanged += ComboBoxType_SelectedIndexChanged;
        comboBoxType.MouseHover += ComboBoxType_MouseHover;
        panelDocument.Controls.Add(comboBoxType);

        buttonAddType = new RoundedButton();
        buttonAddType.Text = "+";
        buttonAddType.Location = new Point(345, 132);
        buttonAddType.Size = new Size(35, 26);
        StyleSmallButton(buttonAddType, Color.FromArgb(52, 152, 219));
        buttonAddType.Click += ButtonAddType_Click;
        panelDocument.Controls.Add(buttonAddType);

        labelOwner = new Label();
        labelOwner.Text = "Владелец:";
        labelOwner.Location = new Point(20, 175);
        labelOwner.AutoSize = true;
        StyleLabel(labelOwner);
        panelDocument.Controls.Add(labelOwner);

        comboBoxOwner = new ComboBox();
        comboBoxOwner.Location = new Point(210, 172);
        comboBoxOwner.Size = new Size(125, 25);
        comboBoxOwner.DropDownStyle = ComboBoxStyle.DropDownList;
        StyleComboBox(comboBoxOwner);
        panelDocument.Controls.Add(comboBoxOwner);

        ownerContextMenu = new ContextMenuStrip();

        ToolStripMenuItem deleteOwnerMenuItem = new ToolStripMenuItem("Удалить владельца");
        deleteOwnerMenuItem.Click += DeleteOwnerMenuItem_Click;

        ownerContextMenu.Items.Add(deleteOwnerMenuItem);
        comboBoxOwner.ContextMenuStrip = ownerContextMenu;

        buttonAddOwner = new RoundedButton();
        buttonAddOwner.Text = "+";
        buttonAddOwner.Location = new Point(345, 172);
        buttonAddOwner.Size = new Size(35, 26);
        StyleSmallButton(buttonAddOwner, Color.FromArgb(52, 152, 219));
        buttonAddOwner.Click += ButtonAddOwner_Click;
        panelDocument.Controls.Add(buttonAddOwner);

        labelDocumentNumber = new Label();
        labelDocumentNumber.Text = "Номер документа:";
        labelDocumentNumber.Location = new Point(20, 215);
        labelDocumentNumber.AutoSize = true;
        StyleLabel(labelDocumentNumber);
        panelDocument.Controls.Add(labelDocumentNumber);

        textBoxDocumentNumber = new TextBox();
        textBoxDocumentNumber.Location = new Point(210, 212);
        textBoxDocumentNumber.Size = new Size(170, 25);
        StyleTextBox(textBoxDocumentNumber);
        panelDocument.Controls.Add(textBoxDocumentNumber);

        labelDate = new Label();
        labelDate.Text = "Дата документа:";
        labelDate.Location = new Point(20, 255);
        labelDate.AutoSize = true;
        StyleLabel(labelDate);
        panelDocument.Controls.Add(labelDate);

        dateTimePickerDocument = new DateTimePicker();
        dateTimePickerDocument.Location = new Point(210, 252);
        dateTimePickerDocument.Size = new Size(170, 25);
        dateTimePickerDocument.Format = DateTimePickerFormat.Custom;
        dateTimePickerDocument.CustomFormat = "dd.MM.yyyy";
        dateTimePickerDocument.ValueChanged += DateTimePickerDocument_ValueChanged;
        panelDocument.Controls.Add(dateTimePickerDocument);

        labelExpirationDate = new Label();
        labelExpirationDate.Text = "Срок действия:";
        labelExpirationDate.Location = new Point(20, 295);
        labelExpirationDate.AutoSize = true;
        labelExpirationDate.Visible = false;
        StyleLabel(labelExpirationDate);
        panelDocument.Controls.Add(labelExpirationDate);

        dateTimePickerExpiration = new DateTimePicker();
        dateTimePickerExpiration.Location = new Point(210, 292);
        dateTimePickerExpiration.Size = new Size(170, 25);
        dateTimePickerExpiration.Format = DateTimePickerFormat.Custom;
        dateTimePickerExpiration.CustomFormat = "dd.MM.yyyy";
        dateTimePickerExpiration.Visible = false;
        panelDocument.Controls.Add(dateTimePickerExpiration);

        checkBoxHasExpiration = new CheckBox();
        checkBoxHasExpiration.Text = "Есть срок действия";
        checkBoxHasExpiration.Location = new Point(210, 322);
        checkBoxHasExpiration.AutoSize = true;
        checkBoxHasExpiration.ForeColor = Color.FromArgb(44, 62, 80);
        checkBoxHasExpiration.CheckedChanged += CheckBoxHasExpiration_CheckedChanged;
        panelDocument.Controls.Add(checkBoxHasExpiration);

        labelComment = new Label();
        labelComment.Text = "Комментарий:";
        labelComment.Location = new Point(20, 355);
        labelComment.AutoSize = true;
        StyleLabel(labelComment);
        panelDocument.Controls.Add(labelComment);

        textBoxComment = new TextBox();
        textBoxComment.Location = new Point(210, 352);
        textBoxComment.Size = new Size(170, 55);
        textBoxComment.Multiline = true;
        StyleTextBox(textBoxComment);
        panelDocument.Controls.Add(textBoxComment);

        labelFile = new Label();
        labelFile.Text = "Файл:";
        labelFile.Location = new Point(20, 420);
        labelFile.AutoSize = true;
        StyleLabel(labelFile);
        panelDocument.Controls.Add(labelFile);

        textBoxFilePath = new TextBox();
        textBoxFilePath.Location = new Point(210, 417);
        textBoxFilePath.Size = new Size(170, 25);
        textBoxFilePath.ReadOnly = true;
        StyleTextBox(textBoxFilePath);
        panelDocument.Controls.Add(textBoxFilePath);

        buttonOpenFile = new RoundedButton();
        buttonOpenFile.Text = "Открыть файл";
        buttonOpenFile.Location = new Point(20, 455);
        buttonOpenFile.Size = new Size(170, 38);
        StyleSmallButton(buttonOpenFile, Color.FromArgb(52, 73, 94));
        buttonOpenFile.Click += ButtonOpenFile_Click;
        panelDocument.Controls.Add(buttonOpenFile);

        buttonSelectFile = new RoundedButton();
        buttonSelectFile.Text = "Выбрать файл";
        buttonSelectFile.Location = new Point(210, 455);
        buttonSelectFile.Size = new Size(170, 38);
        StyleSmallButton(buttonSelectFile, Color.FromArgb(155, 89, 182));
        buttonSelectFile.Click += ButtonSelectFile_Click;
        panelDocument.Controls.Add(buttonSelectFile);

        checkBoxImportant = new CheckBox();
        checkBoxImportant.Text = "Важный документ";
        checkBoxImportant.Location = new Point(210, 505);
        checkBoxImportant.AutoSize = true;
        checkBoxImportant.ForeColor = Color.FromArgb(44, 62, 80);
        checkBoxImportant.CheckedChanged += CheckBoxImportant_CheckedChanged;
        panelDocument.Controls.Add(checkBoxImportant);

        buttonAdd = new RoundedButton();
        buttonAdd.Text = "Добавить";
        buttonAdd.Location = new Point(20, 540);
        buttonAdd.Size = new Size(170, 40);
        StyleButton(buttonAdd, Color.FromArgb(52, 152, 219));
        buttonAdd.Click += ButtonAdd_Click;
        panelDocument.Controls.Add(buttonAdd);

        buttonClear = new RoundedButton();
        buttonClear.Text = "Очистить";
        buttonClear.Location = new Point(210, 540);
        buttonClear.Size = new Size(170, 40);
        StyleButton(buttonClear, Color.FromArgb(149, 165, 166));
        buttonClear.Click += ButtonClear_Click;
        panelDocument.Controls.Add(buttonClear);

        buttonDeleteAll = new RoundedButton();
        buttonDeleteAll.Text = "Удалить все";
        buttonDeleteAll.Location = new Point(20, 590);
        buttonDeleteAll.Size = new Size(170, 40);
        StyleButton(buttonDeleteAll, Color.FromArgb(192, 57, 43));
        buttonDeleteAll.Click += ButtonDeleteAll_Click;
        panelDocument.Controls.Add(buttonDeleteAll);

        buttonEdit = new RoundedButton();
        buttonEdit.Text = "Изменить";
        buttonEdit.Location = new Point(210, 590);
        buttonEdit.Size = new Size(170, 40);
        StyleButton(buttonEdit, Color.FromArgb(46, 204, 113));
        buttonEdit.Click += ButtonEdit_Click;
        panelDocument.Controls.Add(buttonEdit);

        labelSearch = new Label();
        labelSearch.Text = "Поиск:";
        labelSearch.Location = new Point(10, 55);
        labelSearch.AutoSize = true;
        StyleLabel(labelSearch);
        panelDocuments.Controls.Add(labelSearch);

        textBoxSearch = new TextBox();
        textBoxSearch.Location = new Point(80, 52);
        textBoxSearch.Size = new Size(190, 25);
        StyleTextBox(textBoxSearch);
        textBoxSearch.TextChanged += SearchControls_Changed;
        panelDocuments.Controls.Add(textBoxSearch);

        labelFilter = new Label();
        labelFilter.Text = "Тип:";
        labelFilter.Location = new Point(285, 55);
        labelFilter.AutoSize = true;
        StyleLabel(labelFilter);
        panelDocuments.Controls.Add(labelFilter);

        comboBoxFilter = new ComboBox();
        comboBoxFilter.Location = new Point(335, 52);
        comboBoxFilter.Size = new Size(185, 25);
        comboBoxFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        StyleComboBox(comboBoxFilter);
        comboBoxFilter.SelectedIndexChanged += SearchControls_Changed;
        comboBoxFilter.MouseHover += ComboBoxFilter_MouseHover;
        panelDocuments.Controls.Add(comboBoxFilter);

        labelOwnerFilter = new Label();
        labelOwnerFilter.Text = "Владелец:";
        labelOwnerFilter.Location = new Point(525, 55);
        labelOwnerFilter.AutoSize = true;
        StyleLabel(labelOwnerFilter);
        panelDocuments.Controls.Add(labelOwnerFilter);

        comboBoxOwnerFilter = new ComboBox();
        comboBoxOwnerFilter.Location = new Point(620, 52);
        comboBoxOwnerFilter.Size = new Size(125, 25);
        comboBoxOwnerFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        StyleComboBox(comboBoxOwnerFilter);
        comboBoxOwnerFilter.SelectedIndexChanged += SearchControls_Changed;
        panelDocuments.Controls.Add(comboBoxOwnerFilter);

        checkBoxOnlyImportant = new CheckBox();
        checkBoxOnlyImportant.Text = "Только важные";
        checkBoxOnlyImportant.Location = new Point(20, 90);
        checkBoxOnlyImportant.AutoSize = true;
        checkBoxOnlyImportant.ForeColor = Color.FromArgb(44, 62, 80);
        checkBoxOnlyImportant.CheckedChanged += SearchControls_Changed;
        panelDocuments.Controls.Add(checkBoxOnlyImportant);

        dataGridViewDocuments = new DataGridView();
        dataGridViewDocuments.Location = new Point(20, 130);
        dataGridViewDocuments.Size = new Size(725, 420);
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

        dataGridViewDocuments.Columns["Id"]!.Width = 50;
        dataGridViewDocuments.Columns["Title"]!.Width = 220;
        dataGridViewDocuments.Columns["Category"]!.Width = 150;
        dataGridViewDocuments.Columns["Date"]!.Width = 100;
        dataGridViewDocuments.Columns["Status"]!.Width = 90;
        dataGridViewDocuments.Columns["ExpirationStatus"]!.Width = 110;

        StyleDataGridView();
        panelDocuments.Controls.Add(dataGridViewDocuments);

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

        buttonStatistics = new RoundedButton();
        buttonStatistics.Text = "Статистика";
        buttonStatistics.Location = new Point(385, 580);
        buttonStatistics.Size = new Size(170, 40);
        StyleButton(buttonStatistics, Color.FromArgb(41, 128, 185));
        buttonStatistics.Click += ButtonStatistics_Click;
        panelDocuments.Controls.Add(buttonStatistics);

        buttonResetSearch = new RoundedButton();
        buttonResetSearch.Text = "Сбросить";
        buttonResetSearch.Location = new Point(575, 580);
        buttonResetSearch.Size = new Size(170, 40);
        StyleButton(buttonResetSearch, Color.FromArgb(149, 165, 166));
        buttonResetSearch.Click += ButtonResetSearch_Click;
        panelDocuments.Controls.Add(buttonResetSearch);

        labelInfo = new TextBox();
        labelInfo.Text = "Информация о документе появится здесь.";
        labelInfo.Location = new Point(20, 48);
        labelInfo.Size = new Size(1165, 105);
        labelInfo.BackColor = Color.White;
        labelInfo.ForeColor = Color.FromArgb(44, 62, 80);
        labelInfo.BorderStyle = BorderStyle.None;
        labelInfo.Multiline = true;
        labelInfo.ReadOnly = true;
        labelInfo.ScrollBars = ScrollBars.Vertical;
        labelInfo.Font = new Font("Segoe UI", 10);
        panelInfo.Controls.Add(labelInfo);
    }

    private void StyleButton(RoundedButton button, Color backColor)
    {
        button.BackColor = backColor;
        button.ForeColor = Color.White;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        button.Cursor = Cursors.Hand;
        button.CornerRadius = 10;
    }

    private void StyleSmallButton(RoundedButton button, Color backColor)
    {
        button.BackColor = backColor;
        button.ForeColor = Color.White;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        button.Cursor = Cursors.Hand;
        button.CornerRadius = 8;
    }

    private void StyleLabel(Label label)
    {
        label.ForeColor = Color.FromArgb(44, 62, 80);
        label.Font = new Font("Segoe UI", 9, FontStyle.Regular);
    }

    private void StyleTextBox(TextBox textBox)
    {
        textBox.BackColor = Color.White;
        textBox.ForeColor = Color.FromArgb(44, 62, 80);
        textBox.Font = new Font("Segoe UI", 9);
        textBox.BorderStyle = BorderStyle.FixedSingle;
    }

    private void StyleComboBox(ComboBox comboBox)
    {
        comboBox.BackColor = Color.White;
        comboBox.ForeColor = Color.FromArgb(44, 62, 80);
        comboBox.Font = new Font("Segoe UI", 9);
    }

    private Panel CreateShadowPanel(Point location, Size size)
    {
        RoundedPanel shadow = new RoundedPanel();
        shadow.Location = new Point(location.X + 5, location.Y + 6);
        shadow.Size = size;
        shadow.BackColor = Color.FromArgb(210, 218, 225);
        shadow.BorderColor = Color.FromArgb(210, 218, 225);
        shadow.CornerRadius = 18;
        shadow.Enabled = false;

        return shadow;
    }

    private void StyleDataGridView()
    {
        dataGridViewDocuments.BackgroundColor = Color.White;
        dataGridViewDocuments.BorderStyle = BorderStyle.None;
        dataGridViewDocuments.EnableHeadersVisualStyles = false;

        dataGridViewDocuments.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
        dataGridViewDocuments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dataGridViewDocuments.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        dataGridViewDocuments.ColumnHeadersHeight = 32;

        dataGridViewDocuments.DefaultCellStyle.BackColor = Color.White;
        dataGridViewDocuments.DefaultCellStyle.ForeColor = Color.FromArgb(44, 62, 80);
        dataGridViewDocuments.DefaultCellStyle.Font = new Font("Segoe UI", 9);
        dataGridViewDocuments.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
        dataGridViewDocuments.DefaultCellStyle.SelectionForeColor = Color.White;

        dataGridViewDocuments.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        dataGridViewDocuments.GridColor = Color.FromArgb(230, 230, 230);
        dataGridViewDocuments.RowTemplate.Height = 30;
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
                document.DocumentDate.ToString("dd.MM.yyyy"),
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
        int nextId = GetNextDocumentId();

        if (nextId > numericId.Maximum)
        {
            numericId.Maximum = nextId;
        }

        numericId.Value = nextId;
    }

    private void RefreshOwnersList()
    {
        string selectedOwner = comboBoxOwner.SelectedItem?.ToString() ?? "";
        string selectedOwnerFilter = comboBoxOwnerFilter.SelectedItem?.ToString() ?? "Все";

        comboBoxOwner.Items.Clear();
        comboBoxOwnerFilter.Items.Clear();

        comboBoxOwnerFilter.Items.Add("Все");

        foreach (string owner in owners)
        {
            comboBoxOwner.Items.Add(owner);
            comboBoxOwnerFilter.Items.Add(owner);
        }

        if (!string.IsNullOrWhiteSpace(selectedOwner) && comboBoxOwner.Items.Contains(selectedOwner))
        {
            comboBoxOwner.SelectedItem = selectedOwner;
        }
        else if (comboBoxOwner.Items.Count > 0)
        {
            comboBoxOwner.SelectedIndex = 0;
        }

        if (!string.IsNullOrWhiteSpace(selectedOwnerFilter) && comboBoxOwnerFilter.Items.Contains(selectedOwnerFilter))
        {
            comboBoxOwnerFilter.SelectedItem = selectedOwnerFilter;
        }
        else
        {
            comboBoxOwnerFilter.SelectedIndex = 0;
        }
    }

    private void RefreshDocumentTypesList()
    {
        List<string> defaultTypes = new List<string>
        {
            "Паспорт",
            "Свидетельство",
            "Медицинский документ",
            "Договор",
            "Квитанция"
        };

        List<string> allTypes = defaultTypes
            .Concat(documentTypes)
            .Distinct()
            .ToList();

        comboBoxType.Items.Clear();
        comboBoxFilter.Items.Clear();

        comboBoxFilter.Items.Add("Все");

        foreach (string type in allTypes)
        {
            comboBoxType.Items.Add(type);
            comboBoxFilter.Items.Add(type);
        }

        if (comboBoxType.Items.Count > 0)
        {
            comboBoxType.SelectedIndex = 0;
        }

        comboBoxFilter.SelectedIndex = 0;
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
            ? document.ExpirationDate.Value.ToString("dd.MM.yyyy")
            : "не указан";

        return
            "ID: " + document.Id + Environment.NewLine +
            "Название: " + document.Title + Environment.NewLine +
            "Категория: " + document.Category + Environment.NewLine +
            "Владелец: " + document.Owner + Environment.NewLine +
            "Номер документа: " + document.DocumentNumber + Environment.NewLine +
            "Дата документа: " + document.DocumentDate.ToString("dd.MM.yyyy") + Environment.NewLine +
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
        labelInfo.Text = "Выбрана дата документа: " + dateTimePickerDocument.Value.ToString("dd.MM.yyyy");
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
                ? dateTimePickerExpiration.Value.Date
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
            ? dateTimePickerExpiration.Value.Date
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
        if (isUpdatingControls)
        {
            return;
        }
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
        comboBoxOwnerFilter.SelectedIndex = 0;
        checkBoxOnlyImportant.Checked = false;

        SetNextDocumentId();
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

        isUpdatingControls = true;

        RefreshOwnersList();
        comboBoxOwner.SelectedItem = ownerName;

        if (dataGridViewDocuments.CurrentRow != null)
        {
            dataGridViewDocuments.ClearSelection();
        }

        isUpdatingControls = false;

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

    private void ButtonAddType_Click(object? sender, EventArgs e)
    {
        if (userRole == "viewer")
        {
            labelInfo.Text = "Недостаточно прав для добавления типа документа.";
            return;
        }

        string typeName = Microsoft.VisualBasic.Interaction.InputBox(
            "Введите новый тип документа:",
            "Добавление типа документа",
            ""
        ).Trim();

        if (string.IsNullOrWhiteSpace(typeName))
        {
            labelInfo.Text = "Тип документа не введен.";
            return;
        }

        bool typeExists = comboBoxType.Items
            .Cast<object>()
            .Any(item => item.ToString() == typeName);

        if (typeExists)
        {
            labelInfo.Text = "Такой тип документа уже есть в списке.";
            return;
        }

        documentTypes.Add(typeName);
        documentTypeStorageService.SaveTypes(documentTypes);

        RefreshDocumentTypesList();

        comboBoxType.SelectedItem = typeName;
        labelInfo.Text = "Тип документа добавлен: " + typeName;
    }

    private void ComboBoxType_MouseHover(object? sender, EventArgs e)
    {
        string selectedType = comboBoxType.SelectedItem?.ToString() ?? "";

        if (!string.IsNullOrWhiteSpace(selectedType))
        {
            toolTip.SetToolTip(comboBoxType, selectedType);
        }
    }

    private void ComboBoxFilter_MouseHover(object? sender, EventArgs e)
    {
        string selectedType = comboBoxFilter.SelectedItem?.ToString() ?? "";

        if (!string.IsNullOrWhiteSpace(selectedType))
        {
            toolTip.SetToolTip(comboBoxFilter, selectedType);
        }
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
            buttonAddType.Enabled = false;

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