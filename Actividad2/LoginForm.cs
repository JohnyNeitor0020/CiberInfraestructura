using System.Text.RegularExpressions;
namespace SistemaInventario;

public class LoginForm : Form
{
    private readonly TextBox txtPass = Estilo.Caja(360), txtConf = Estilo.Caja(360);
    private readonly Label rLen = new(), rMay = new(), rMin = new(), rNum = new(), rSim = new(), rIgual = new();

    public LoginForm()
    {
        Text = "Sistema de Inventario - Acceso";
        Font = Estilo.Fuente; BackColor = Color.White;
        ClientSize = new Size(440, 510);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false;

        // Encabezado
        var header = new Panel { Dock = DockStyle.Top, Height = 120, BackColor = Estilo.Oscuro };
        header.Controls.Add(new Label
        {
            Text = "SISTEMA DE INVENTARIO", Dock = DockStyle.Fill, ForeColor = Color.White,
            Font = new Font("Segoe UI", 17F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter
        });
        header.Controls.Add(new Label
        {
            Text = "Control de acceso", Dock = DockStyle.Bottom, Height = 36,
            ForeColor = Color.FromArgb(148, 163, 184), TextAlign = ContentAlignment.TopCenter
        });
        Controls.Add(header);

        // Campos
        txtPass.UseSystemPasswordChar = true; txtConf.UseSystemPasswordChar = true;
        Poner(new Label { Text = "Contraseña", AutoSize = true, ForeColor = Estilo.Gris }, 40, 145);
        Poner(txtPass, 40, 168);
        Poner(new Label { Text = "Confirmar contraseña", AutoSize = true, ForeColor = Estilo.Gris }, 40, 212);
        Poner(txtConf, 40, 235);

        var chk = new CheckBox { Text = "Mostrar contraseñas", AutoSize = true, ForeColor = Estilo.Gris };
        chk.CheckedChanged += (s, e) => txtPass.UseSystemPasswordChar = txtConf.UseSystemPasswordChar = !chk.Checked;
        Poner(chk, 40, 280);

        // Requisitos en vivo
        Poner(rLen, 40, 320); Poner(rMay, 40, 346); Poner(rMin, 40, 372);
        Poner(rNum, 230, 320); Poner(rSim, 230, 346); Poner(rIgual, 230, 372);
        foreach (var l in new[] { rLen, rMay, rMin, rNum, rSim, rIgual }) l.AutoSize = true;
        txtPass.TextChanged += (s, e) => Revisar();
        txtConf.TextChanged += (s, e) => Revisar();
        Revisar();

        var btn = Estilo.Boton("Validar contraseña", Estilo.Primario, Validar, 360);
        btn.Height = 48;
        Poner(btn, 40, 425);
        AcceptButton = btn;
    }

    private void Poner(Control c, int x, int y) { c.Location = new Point(x, y); Controls.Add(c); }

    private static bool Marcar(Label l, string texto, bool ok)
    {
        l.Text = (ok ? "✓  " : "○  ") + texto;
        l.ForeColor = ok ? Estilo.Verde : Color.Gray;
        return ok;
    }

    private bool Revisar()
    {
        string p = txtPass.Text;
        bool a = Marcar(rLen, "Mínimo 8 caracteres", p.Length >= 8);
        bool b = Marcar(rMay, "Una mayúscula", Regex.IsMatch(p, "[A-Z]"));
        bool c = Marcar(rMin, "Una minúscula", Regex.IsMatch(p, "[a-z]"));
        bool d = Marcar(rNum, "Un número", Regex.IsMatch(p, "[0-9]"));
        bool e = Marcar(rSim, "Un símbolo", Regex.IsMatch(p, @"[^\w\s]"));
        bool f = Marcar(rIgual, "Las contraseñas coinciden", p.Length > 0 && p == txtConf.Text);
        return a && b && c && d && e && f;
    }

    private void Validar()
    {
        if (!Revisar())
        {
            MessageBox.Show("La contraseña no cumple los criterios de seguridad.", "Acceso denegado",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        MessageBox.Show("Contraseña validada correctamente.", "Acceso autorizado",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK;
    }
}
