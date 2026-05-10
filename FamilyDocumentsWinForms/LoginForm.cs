using System;
using System.Drawing;
using System.Windows.Forms;
using FamilyDocumentsWinForms.Controls;

namespace FamilyDocumentsWinForms;

public partial class LoginForm : Form
{
    private RoundedPanel panelLogin = null!;
    private Panel shadowLogin = null!;

    private Label labelTitle = null!;
    private Label labelSubtitle = null!;
    private Label labelLogin = null!;
    private Label labelPassword = null!;
    private Label labelInfo = null!;

    private TextBox textBoxLogin = null!;
    private TextBox textBoxPassword = null!;
    private TextBox textBoxHint = null!;

    private RoundedButton buttonLogin = null!;
    private RoundedButton buttonExit = null!;

    public LoginForm()
    {
        CreateFormElements();
    }

    private void CreateFormElements()
    {
        this.Text = "Вход в систему";
        this.Size = new Size(520, 420);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(245, 247, 250);
        this.Font = new Font("Segoe UI", 9);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;

        shadowLogin = CreateShadowPanel(new Point(45, 45), new Size(415, 305));
        this.Controls.Add(shadowLogin);

        panelLogin = new RoundedPanel();
        panelLogin.Location = new Point(40, 40);
        panelLogin.Size = new Size(415, 305);
        panelLogin.CornerRadius = 18;
        panelLogin.BorderColor = Color.FromArgb(225, 230, 235);
        this.Controls.Add(panelLogin);
        panelLogin.BringToFront();

        labelTitle = new Label();
        labelTitle.Text = "Вход в систему";
        labelTitle.Location = new Point(75, 15);
        labelTitle.AutoSize = true;
        labelTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
        labelTitle.ForeColor = Color.FromArgb(44, 62, 80);
        panelLogin.Controls.Add(labelTitle);

        labelSubtitle = new Label();
        labelSubtitle.Text = "Введите логин и пароль для продолжения";
        labelSubtitle.Location = new Point(32, 62);
        labelSubtitle.AutoSize = true;
        labelSubtitle.Font = new Font("Segoe UI", 9);
        labelSubtitle.ForeColor = Color.FromArgb(100, 110, 120);
        panelLogin.Controls.Add(labelSubtitle);

        labelLogin = new Label();
        labelLogin.Text = "Логин:";
        labelLogin.Location = new Point(35, 105);
        labelLogin.AutoSize = true;
        StyleLabel(labelLogin);
        panelLogin.Controls.Add(labelLogin);

        textBoxLogin = new TextBox();
        textBoxLogin.Location = new Point(135, 102);
        textBoxLogin.Size = new Size(235, 25);
        StyleTextBox(textBoxLogin);
        textBoxLogin.KeyDown += TextBox_KeyDown;
        panelLogin.Controls.Add(textBoxLogin);

        labelPassword = new Label();
        labelPassword.Text = "Пароль:";
        labelPassword.Location = new Point(35, 145);
        labelPassword.AutoSize = true;
        StyleLabel(labelPassword);
        panelLogin.Controls.Add(labelPassword);

        textBoxPassword = new TextBox();
        textBoxPassword.Location = new Point(135, 142);
        textBoxPassword.Size = new Size(235, 25);
        textBoxPassword.PasswordChar = '*';
        StyleTextBox(textBoxPassword);
        textBoxPassword.KeyDown += TextBox_KeyDown;
        panelLogin.Controls.Add(textBoxPassword);

        textBoxHint = new TextBox();
        textBoxHint.Location = new Point(35, 185);
        textBoxHint.Size = new Size(335, 48);
        textBoxHint.Multiline = true;
        textBoxHint.ReadOnly = true;
        textBoxHint.BorderStyle = BorderStyle.None;
        textBoxHint.BackColor = Color.White;
        textBoxHint.ForeColor = Color.FromArgb(100, 110, 120);
        textBoxHint.Font = new Font("Segoe UI", 9);
        textBoxHint.Text =
            "admin/admin — администратор" + Environment.NewLine +
            "viewer/viewer — просмотр";
        panelLogin.Controls.Add(textBoxHint);

        buttonLogin = new RoundedButton();
        buttonLogin.Text = "Войти";
        buttonLogin.Location = new Point(35, 245);
        buttonLogin.Size = new Size(160, 38);
        StyleButton(buttonLogin, Color.FromArgb(52, 152, 219));
        buttonLogin.Click += ButtonLogin_Click;
        panelLogin.Controls.Add(buttonLogin);

        buttonExit = new RoundedButton();
        buttonExit.Text = "Выход";
        buttonExit.Location = new Point(210, 245);
        buttonExit.Size = new Size(160, 38);
        StyleButton(buttonExit, Color.FromArgb(149, 165, 166));
        buttonExit.Click += ButtonExit_Click;
        panelLogin.Controls.Add(buttonExit);

        labelInfo = new Label();
        labelInfo.Text = "";
        labelInfo.Location = new Point(40, 360);
        labelInfo.Size = new Size(415, 25);
        labelInfo.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        labelInfo.ForeColor = Color.FromArgb(192, 57, 43);
        labelInfo.TextAlign = ContentAlignment.MiddleCenter;
        this.Controls.Add(labelInfo);

        textBoxLogin.Focus();
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

    private void TextBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            ButtonLogin_Click(sender, e);
        }
    }

    private void ButtonLogin_Click(object? sender, EventArgs e)
    {
        string login = textBoxLogin.Text.Trim();
        string password = textBoxPassword.Text.Trim();

        if (login == "admin" && password == "admin")
        {
            OpenMainForm("admin");
            return;
        }

        if (login == "viewer" && password == "viewer")
        {
            OpenMainForm("viewer");
            return;
        }

        labelInfo.Text = "Неверный логин или пароль.";
        textBoxPassword.Clear();
        textBoxPassword.Focus();
    }

    private void OpenMainForm(string role)
    {
        Form1 mainForm = new Form1(role);

        mainForm.FormClosed += (sender, e) =>
        {
            this.Close();
        };

        this.Hide();
        mainForm.Show();
    }

    private void ButtonExit_Click(object? sender, EventArgs e)
    {
        Application.Exit();
    }
}