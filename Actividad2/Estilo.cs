namespace SistemaInventario;

public static class Estilo
{
    public static readonly Color Primario = Color.FromArgb(37, 99, 235);
    public static readonly Color Oscuro = Color.FromArgb(15, 23, 42);
    public static readonly Color Fondo = Color.FromArgb(241, 245, 249);
    public static readonly Color Verde = Color.FromArgb(22, 163, 74);
    public static readonly Color Rojo = Color.FromArgb(220, 38, 38);
    public static readonly Color Gris = Color.FromArgb(100, 116, 139);
    public static readonly Font Fuente = new("Segoe UI", 10F);

    public static Button Boton(string texto, Color color, Action accion, int ancho = 140)
    {
        var b = new Button
        {
            Text = texto, Width = ancho, Height = 40, BackColor = color, ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Font = new Font("Segoe UI", 10F, FontStyle.Bold)
        };
        b.FlatAppearance.BorderSize = 0;
        b.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(color, 0.1f);
        b.Click += (s, e) => accion();
        return b;
    }

    public static TextBox Caja(int ancho) =>
        new() { Width = ancho, Font = new Font("Segoe UI", 11F), BorderStyle = BorderStyle.FixedSingle };

    public static void Tabla(DataGridView g)
    {
        g.Dock = DockStyle.Fill; g.ReadOnly = true; g.AllowUserToAddRows = false; g.MultiSelect = false;
        g.BorderStyle = BorderStyle.None; g.BackgroundColor = Color.White; g.RowHeadersVisible = false;
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        g.EnableHeadersVisualStyles = false;
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        g.ColumnHeadersHeight = 40;
        g.ColumnHeadersDefaultCellStyle.BackColor = Oscuro;
        g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        g.RowTemplate.Height = 36;
        g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        g.GridColor = Color.FromArgb(226, 232, 240);
        g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
        g.DefaultCellStyle.SelectionBackColor = Primario;
        g.DefaultCellStyle.SelectionForeColor = Color.White;
        g.DefaultCellStyle.Font = new Font("Segoe UI", 10.5F);
    }
}
