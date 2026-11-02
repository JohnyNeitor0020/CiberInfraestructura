namespace UtileriaLLM;

public class MainForm : Form
{
    private readonly LlmClient cliente = new();
    private readonly TextBox txtKey = new() { Width = 300, UseSystemPasswordChar = true };
    private readonly TextBox txtModelo = new() { Width = 200, Text = "gemini-2.5-flash" };
    private readonly TextBox txtPrompt = new() { Multiline = true, Dock = DockStyle.Fill, Font = new Font("Segoe UI", 11F) };
    private readonly TextBox txtRespuesta = new() { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Fill, Font = new Font("Segoe UI", 11F), BackColor = Color.White };
    private readonly TextBox txtDetalle = new() { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Both, Dock = DockStyle.Fill, Font = new Font("Consolas", 9F), BackColor = Color.White };
    private readonly Button btn = new() { Text = "Enviar a la IA", Height = 42, Width = 180, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(37, 99, 235), ForeColor = Color.White, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };

    public MainForm()
    {
        Text = "Utilería API Call a LLM";
        Font = new Font("Segoe UI", 10F);
        Size = new Size(900, 760);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(241, 245, 249);
        btn.FlatAppearance.BorderSize = 0;

        var t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 8, Padding = new Padding(16) };
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.RowStyles.Add(new RowStyle(SizeType.Percent, 40));

        var config = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill };
        config.Controls.Add(new Label { Text = "API Key:", AutoSize = true, Margin = new Padding(3, 6, 3, 0) });
        config.Controls.Add(txtKey);
        config.Controls.Add(new Label { Text = "Modelo:", AutoSize = true, Margin = new Padding(15, 6, 3, 0) });
        config.Controls.Add(txtModelo);

        t.Controls.Add(config, 0, 0);
        t.Controls.Add(Etiqueta("Pregunta (prompt):"), 0, 1);
        t.Controls.Add(txtPrompt, 0, 2);
        t.Controls.Add(btn, 0, 3);
        t.Controls.Add(Etiqueta("Respuesta de la IA:"), 0, 4);
        t.Controls.Add(txtRespuesta, 0, 5);
        t.Controls.Add(Etiqueta("Detalle HTTP (petición y respuesta JSON):"), 0, 6);
        t.Controls.Add(txtDetalle, 0, 7);
        Controls.Add(t);

        btn.Click += async (s, e) => await Enviar();
    }

    private static Label Etiqueta(string texto) =>
        new() { Text = texto, AutoSize = true, ForeColor = Color.FromArgb(100, 116, 139), Margin = new Padding(3, 10, 3, 3) };

    private async Task Enviar()
    {
        if (txtKey.Text.Trim() == "" || txtPrompt.Text.Trim() == "")
        { MessageBox.Show("Escribe tu API key y una pregunta.", "Aviso"); return; }

        btn.Enabled = false; btn.Text = "Consultando...";
        txtRespuesta.Text = "";
        try
        {
            txtRespuesta.Text = await cliente.PreguntarAsync(txtKey.Text.Trim(), txtModelo.Text.Trim(), txtPrompt.Text.Trim());
        }
        catch (Exception ex)
        {
            txtRespuesta.Text = "⚠ " + ex.Message;
        }
        finally
        {
            txtDetalle.Text = $"POST generativelanguage.googleapis.com ... (HTTP {cliente.UltimoCodigo})\r\n\r\n" +
                              $"PETICIÓN:\r\n{cliente.UltimaPeticion}\r\n\r\nRESPUESTA:\r\n{cliente.UltimaRespuesta}";
            btn.Enabled = true; btn.Text = "Enviar a la IA";
        }
    }
}
