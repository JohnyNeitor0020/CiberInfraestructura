namespace SistemaInventario;
static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        var login = new LoginForm();
        if (login.ShowDialog() == DialogResult.OK)
            Application.Run(new MainForm());
    }
}
