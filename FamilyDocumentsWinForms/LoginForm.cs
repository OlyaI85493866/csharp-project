using System;
using System.Drawing;
using System.Windows.Forms;

namespace FamilyDocumentsWinForms;

public class LoginForm : Form
{
    private Panel panelCard = null!;
    private Label labelHeader = null!;
    private Label labelSubtitle = null!;
    private Label labelLogin = null!;
    private Label labelPassword = null!;
    private TextBox textBoxLogin = null!;
    private TextBox textBoxPassword = null!;
    private Button buttonLogin = null!;
    private Button buttonExit = null!;
    private Label labelMessage = null!;
    private TextBox textBoxHint = null!;
    public LoginForm()
    {
        CreateFormElements();
    }

    private void CreateFormElements()
    {
        this.Text = "Вход в систему";
        this.Size = new Size(520, 480);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(245, 247, 250);
        this.Font = new Font("Segoe UI", 9);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;

        panelCard = new Panel();
        panelCard.Location = new Point(45, 35);
        panelCard.Size = new Size(410, 360);
        panelCard.BackColor = Color.White;
        panelCard.BorderStyle = BorderStyle.FixedSingle;
        this.Controls.Add(panelCard);

        labelHeader = new Label();
        labelHeader.Text = "Семейная документация";
        labelHeader.Location = new Point(12, 20);
        labelHeader.AutoSize = true;
        labelHeader.Font = new Font("Segoe UI", 15, FontStyle.Bold);
        labelHeader.ForeColor = Color.FromArgb(44, 62, 80);
        panelCard.Controls.Add(labelHeader);

        labelSubtitle = new Label();
        labelSubtitle.Text = "Вход в автоматизированную систему";
        labelSubtitle.Location = new Point(45, 65);
        labelSubtitle.AutoSize = true;
        labelSubtitle.Font = new Font("Segoe UI", 9);
        labelSubtitle.ForeColor = Color.FromArgb(127, 140, 141);
        panelCard.Controls.Add(labelSubtitle);

        labelLogin = new Label();
        labelLogin.Text = "Логин:";
        labelLogin.Location = new Point(45, 110);
        labelLogin.AutoSize = true;
        labelLogin.ForeColor = Color.FromArgb(44, 62, 80);
        panelCard.Controls.Add(labelLogin);

        textBoxLogin = new TextBox();
        textBoxLogin.Location = new Point(145, 107);
        textBoxLogin.Size = new Size(210, 25);
        textBoxLogin.BorderStyle = BorderStyle.FixedSingle;
        textBoxLogin.Font = new Font("Segoe UI", 10);
        panelCard.Controls.Add(textBoxLogin);

        labelPassword = new Label();
        labelPassword.Text = "Пароль:";
        labelPassword.Location = new Point(45, 150);
        labelPassword.AutoSize = true;
        labelPassword.ForeColor = Color.FromArgb(44, 62, 80);
        panelCard.Controls.Add(labelPassword);

        textBoxPassword = new TextBox();
        textBoxPassword.Location = new Point(145, 147);
        textBoxPassword.Size = new Size(210, 25);
        textBoxPassword.BorderStyle = BorderStyle.FixedSingle;
        textBoxPassword.Font = new Font("Segoe UI", 10);
        textBoxPassword.UseSystemPasswordChar = true;
        textBoxPassword.KeyDown += TextBoxPassword_KeyDown;
        panelCard.Controls.Add(textBoxPassword);

        buttonLogin = new Button();
        buttonLogin.Text = "Войти";
        buttonLogin.Location = new Point(45, 205);
        buttonLogin.Size = new Size(145, 40);
        StyleButton(buttonLogin, Color.FromArgb(52, 152, 219));
        buttonLogin.Click += ButtonLogin_Click;
        panelCard.Controls.Add(buttonLogin);

        buttonExit = new Button();
        buttonExit.Text = "Выход";
        buttonExit.Location = new Point(210, 205);
        buttonExit.Size = new Size(145, 40);
        StyleButton(buttonExit, Color.FromArgb(149, 165, 166));
        buttonExit.Click += ButtonExit_Click;
        panelCard.Controls.Add(buttonExit);

        labelMessage = new Label();
        labelMessage.Text = "Введите логин и пароль";
        labelMessage.Location = new Point(45, 255);
        labelMessage.Size = new Size(310, 25);
        labelMessage.ForeColor = Color.FromArgb(44, 62, 80);
        panelCard.Controls.Add(labelMessage);

        textBoxHint = new TextBox();
        textBoxHint.Text =
            "admin/admin — полный доступ" + Environment.NewLine +
            "viewer/viewer — просмотр";
        textBoxHint.Location = new Point(45, 280);
        textBoxHint.Size = new Size(330, 45);
        textBoxHint.ForeColor = Color.FromArgb(127, 140, 141);
        textBoxHint.BackColor = Color.White;
        textBoxHint.BorderStyle = BorderStyle.None;
        textBoxHint.Font = new Font("Segoe UI", 8);
        textBoxHint.ReadOnly = true;
        textBoxHint.Multiline = true;
        textBoxHint.TabStop = false;
        panelCard.Controls.Add(textBoxHint);
    }

    private void StyleButton(Button button, Color backColor)
    {
        button.BackColor = backColor;
        button.ForeColor = Color.White;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        button.Cursor = Cursors.Hand;
    }

    private void ButtonLogin_Click(object? sender, EventArgs e)
    {
        TryLogin();
    }

    private void TextBoxPassword_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            TryLogin();
        }
    }

    private void TryLogin()
    {
        string login = textBoxLogin.Text.Trim();
        string password = textBoxPassword.Text.Trim();

        if (login == "admin" && password == "admin")
        {
            Form1 mainForm = new Form1("admin");

            this.Hide();
            mainForm.ShowDialog();
            this.Close();
        }
        else if (login == "viewer" && password == "viewer")
        {
            Form1 mainForm = new Form1("viewer");

            this.Hide();
            mainForm.ShowDialog();
            this.Close();
        }
        else
        {
            labelMessage.ForeColor = Color.FromArgb(231, 76, 60);
            labelMessage.Text = "Неверный логин или пароль.";
            textBoxPassword.Clear();
            textBoxPassword.Focus();
        }
    }

    private void ButtonExit_Click(object? sender, EventArgs e)
    {
        this.Close();
    }
}