using System;
using System.Drawing;
using System.Windows.Forms;

namespace FamilyDocumentsWinForms;

public class LoginForm : Form
{
    private Label labelHeader = null!;
    private Label labelLogin = null!;
    private Label labelPassword = null!;
    private TextBox textBoxLogin = null!;
    private TextBox textBoxPassword = null!;
    private Button buttonLogin = null!;
    private Button buttonExit = null!;
    private Label labelMessage = null!;

    public LoginForm()
    {
        CreateFormElements();
    }

    private void CreateFormElements()
    {
        this.Text = "Вход в систему";
        this.Size = new Size(420, 330);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(245, 247, 250);
        this.Font = new Font("Segoe UI", 9);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;

        labelHeader = new Label();
        labelHeader.Text = "Авторизация";
        labelHeader.Location = new Point(80, 25);
        labelHeader.AutoSize = true;
        labelHeader.Font = new Font("Segoe UI", 18, FontStyle.Bold);
        labelHeader.ForeColor = Color.FromArgb(44, 62, 80);
        this.Controls.Add(labelHeader);

        labelLogin = new Label();
        labelLogin.Text = "Логин:";
        labelLogin.Location = new Point(50, 90);
        labelLogin.AutoSize = true;
        this.Controls.Add(labelLogin);

        textBoxLogin = new TextBox();
        textBoxLogin.Location = new Point(150, 88);
        textBoxLogin.Size = new Size(200, 25);
        this.Controls.Add(textBoxLogin);

        labelPassword = new Label();
        labelPassword.Text = "Пароль:";
        labelPassword.Location = new Point(50, 130);
        labelPassword.AutoSize = true;
        this.Controls.Add(labelPassword);

        textBoxPassword = new TextBox();
        textBoxPassword.Location = new Point(150, 128);
        textBoxPassword.Size = new Size(200, 25);
        textBoxPassword.UseSystemPasswordChar = true;
        this.Controls.Add(textBoxPassword);

        buttonLogin = new Button();
        buttonLogin.Text = "Войти";
        buttonLogin.Location = new Point(50, 185);
        buttonLogin.Size = new Size(140, 40);
        buttonLogin.BackColor = Color.FromArgb(52, 152, 219);
        buttonLogin.ForeColor = Color.White;
        buttonLogin.FlatStyle = FlatStyle.Flat;
        buttonLogin.FlatAppearance.BorderSize = 0;
        buttonLogin.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        buttonLogin.Cursor = Cursors.Hand;
        buttonLogin.Click += ButtonLogin_Click;
        this.Controls.Add(buttonLogin);

        buttonExit = new Button();
        buttonExit.Text = "Выход";
        buttonExit.Location = new Point(210, 185);
        buttonExit.Size = new Size(140, 40);
        buttonExit.BackColor = Color.FromArgb(149, 165, 166);
        buttonExit.ForeColor = Color.White;
        buttonExit.FlatStyle = FlatStyle.Flat;
        buttonExit.FlatAppearance.BorderSize = 0;
        buttonExit.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        buttonExit.Cursor = Cursors.Hand;
        buttonExit.Click += ButtonExit_Click;
        this.Controls.Add(buttonExit);

        labelMessage = new Label();
        labelMessage.Text = "Введите логин и пароль.";
        labelMessage.Location = new Point(50, 235);
        labelMessage.Size = new Size(300, 35);
        labelMessage.ForeColor = Color.FromArgb(44, 62, 80);
        this.Controls.Add(labelMessage);
    }

    private void ButtonLogin_Click(object? sender, EventArgs e)
    {
        string login = textBoxLogin.Text.Trim();
        string password = textBoxPassword.Text.Trim();

        if (login == "admin" && password == "admin")
        {
            Form1 mainForm = new Form1();

            this.Hide();
            mainForm.ShowDialog();
            this.Close();
        }
        else
        {
            labelMessage.ForeColor = Color.FromArgb(231, 76, 60);
            labelMessage.Text = "Неверный логин или пароль.";
            textBoxPassword.Clear();
        }
    }

    private void ButtonExit_Click(object? sender, EventArgs e)
    {
        this.Close();
    }
}