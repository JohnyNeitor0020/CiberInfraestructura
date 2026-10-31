namespace SistemaInventario;

public class MainForm : Form
{
    private readonly ConexionMySQL db = new();
    private readonly DataGridView dgvPersonal = new(), dgvProducto = new();
    private readonly TextBox txtNombre = Estilo.Caja(240), txtCargo = Estilo.Caja(240),
                             txtDesc = Estilo.Caja(300), txtCant = Estilo.Caja(100);
    private readonly Label lblTotalP = new(), lblTotalR = new(), lblEstado = new();
    private readonly Panel pnlPersonas, pnlProductos;
    private Button btnPers, btnProd;

    public MainForm()
    {
        Text = "Sistema de Inventario";
        Font = Estilo.Fuente; BackColor = Estilo.Fondo;
        Size = new Size(1020, 640); MinimumSize = new Size(900, 560);
        StartPosition = FormStartPosition.CenterScreen;

        pnlPersonas = Seccion("Personas", dgvPersonal, lblTotalP,
            new[] { ("Nombre", txtNombre), ("Cargo", txtCargo) },
            AgregarPersona, EditarPersona, EliminarPersona, () => { txtNombre.Clear(); txtCargo.Clear(); });
        pnlProductos = Seccion("Productos", dgvProducto, lblTotalR,
            new[] { ("Descripción", txtDesc), ("Cantidad", txtCant) },
            AgregarProducto, EditarProducto, EliminarProducto, () => { txtDesc.Clear(); txtCant.Clear(); });

        var host = new Panel { Dock = DockStyle.Fill };
        host.Controls.Add(pnlPersonas); host.Controls.Add(pnlProductos);
        Controls.Add(host);
        Controls.Add(ArmarMenu());
        Mostrar(pnlPersonas, btnPers);

        dgvPersonal.SelectionChanged += (s, e) =>
        {
            if (dgvPersonal.CurrentRow == null) return;
            txtNombre.Text = dgvPersonal.CurrentRow.Cells["Nombre"].Value?.ToString();
            txtCargo.Text = dgvPersonal.CurrentRow.Cells["Cargo"].Value?.ToString();
        };
        dgvProducto.SelectionChanged += (s, e) =>
        {
            if (dgvProducto.CurrentRow == null) return;
            txtDesc.Text = dgvProducto.CurrentRow.Cells["Descripcion"].Value?.ToString();
            txtCant.Text = dgvProducto.CurrentRow.Cells["Cantidad"].Value?.ToString();
        };

        Load += (s, e) =>
        {
            bool ok = db.ProbarConexion();
            lblEstado.Text = ok ? "● Conectado a MySQL" : "● Sin conexión";
            lblEstado.ForeColor = ok ? Color.LightGreen : Color.Salmon;
            if (!ok)
            {
                MessageBox.Show("No se pudo conectar a MySQL. Revisa ConexionMySQL.cs", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            CargarPersonal(); CargarProducto();
        };
    }

    // ---------- Interfaz ----------
    private Panel ArmarMenu()
    {
        var m = new Panel { Dock = DockStyle.Left, Width = 220, BackColor = Estilo.Oscuro };
        btnProd = Nav("📦   Productos", () => Mostrar(pnlProductos, btnProd));
        btnPers = Nav("👥   Personas", () => Mostrar(pnlPersonas, btnPers));
        var salir = Nav("✖   Salir", () => Close());
        salir.Dock = DockStyle.Bottom;
        lblEstado.Dock = DockStyle.Bottom; lblEstado.Height = 40; lblEstado.TextAlign = ContentAlignment.MiddleCenter;
        var titulo = new Label
        {
            Text = "INVENTARIO", Dock = DockStyle.Top, Height = 90, ForeColor = Color.White,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter
        };
        m.Controls.Add(btnProd); m.Controls.Add(btnPers); m.Controls.Add(titulo);
        m.Controls.Add(salir); m.Controls.Add(lblEstado);
        return m;
    }

    private static Button Nav(string texto, Action accion)
    {
        var b = new Button
        {
            Text = "   " + texto, Dock = DockStyle.Top, Height = 52, FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White, BackColor = Estilo.Oscuro, TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI", 11F), Cursor = Cursors.Hand
        };
        b.FlatAppearance.BorderSize = 0;
        b.Click += (s, e) => accion();
        return b;
    }

    private void Mostrar(Panel panel, Button activo)
    {
        pnlPersonas.Visible = panel == pnlPersonas;
        pnlProductos.Visible = panel == pnlProductos;
        btnPers.BackColor = btnPers == activo ? Estilo.Primario : Estilo.Oscuro;
        btnProd.BackColor = btnProd == activo ? Estilo.Primario : Estilo.Oscuro;
    }

    private Panel Seccion(string titulo, DataGridView g, Label total, (string, TextBox)[] campos,
        Action agregar, Action editar, Action eliminar, Action limpiar)
    {
        var root = new Panel { Dock = DockStyle.Fill, BackColor = Estilo.Fondo, Padding = new Padding(28) };
        Estilo.Tabla(g);

        var lbl = new Label
        {
            Text = titulo, Dock = DockStyle.Top, Height = 55, ForeColor = Estilo.Oscuro,
            Font = new Font("Segoe UI", 20F, FontStyle.Bold)
        };
        var card = new Panel { Dock = DockStyle.Top, Height = 105, BackColor = Color.White, Padding = new Padding(16, 12, 16, 8) };
        var fila = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
        foreach (var (et, tb) in campos)
        {
            var col = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true, WrapContents = false };
            col.Controls.Add(new Label { Text = et, AutoSize = true, ForeColor = Estilo.Gris });
            col.Controls.Add(tb);
            fila.Controls.Add(col);
        }
        var botones = new FlowLayoutPanel { AutoSize = true, Margin = new Padding(10, 18, 0, 0) };
        botones.Controls.Add(Estilo.Boton("Agregar", Estilo.Verde, agregar, 110));
        botones.Controls.Add(Estilo.Boton("Actualizar", Estilo.Primario, editar, 110));
        botones.Controls.Add(Estilo.Boton("Eliminar", Estilo.Rojo, eliminar, 110));
        botones.Controls.Add(Estilo.Boton("Limpiar", Estilo.Gris, limpiar, 100));
        fila.Controls.Add(botones);
        card.Controls.Add(fila);

        total.Dock = DockStyle.Bottom; total.Height = 32; total.ForeColor = Estilo.Gris;
        total.TextAlign = ContentAlignment.BottomLeft;

        root.Controls.Add(g);
        root.Controls.Add(total);
        root.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 14 });
        root.Controls.Add(card);
        root.Controls.Add(lbl);
        return root;
    }

    private static int? IdSeleccionado(DataGridView g) =>
        g.CurrentRow == null ? null : Convert.ToInt32(g.CurrentRow.Cells["ID"].Value);
    private static void Info(string m, string t) => MessageBox.Show(m, t, MessageBoxButtons.OK, MessageBoxIcon.Information);
    private static void Aviso(string m) => MessageBox.Show(m, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    private static bool Confirmar(string m) =>
        MessageBox.Show(m, "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

    // ---------- Personal ----------
    private void CargarPersonal()
    {
        dgvPersonal.DataSource = db.Consultar("SELECT ID, Nombre, Cargo FROM CatPersonal ORDER BY ID");
        lblTotalP.Text = $"Total de registros: {dgvPersonal.Rows.Count}";
    }

    private void AgregarPersona()
    {
        if (txtNombre.Text.Trim() == "" || txtCargo.Text.Trim() == "") { Aviso("Escribe nombre y cargo."); return; }
        db.Ejecutar("INSERT INTO CatPersonal (Nombre, Cargo) VALUES (@nombre, @cargo)",
            ("@nombre", txtNombre.Text.Trim()), ("@cargo", txtCargo.Text.Trim()));
        CargarPersonal();
        Info("La persona se guardó correctamente en MySQL.", "Registro exitoso");
    }

    private void EditarPersona()
    {
        var id = IdSeleccionado(dgvPersonal);
        if (id == null) { Aviso("Selecciona una persona."); return; }
        db.Ejecutar("UPDATE CatPersonal SET Nombre = @nombre, Cargo = @cargo WHERE ID = @id",
            ("@nombre", txtNombre.Text.Trim()), ("@cargo", txtCargo.Text.Trim()), ("@id", id.Value));
        CargarPersonal();
        Info("Los datos del personal se actualizaron correctamente.", "Actualización exitosa");
    }

    private void EliminarPersona()
    {
        var id = IdSeleccionado(dgvPersonal);
        if (id == null) { Aviso("Selecciona una persona."); return; }
        if (!Confirmar("¿Deseas eliminar la persona seleccionada?")) return;
        db.Ejecutar("DELETE FROM CatPersonal WHERE ID = @id", ("@id", id.Value));
        CargarPersonal();
    }

    // ---------- Producto ----------
    private void CargarProducto()
    {
        dgvProducto.DataSource = db.Consultar("SELECT ID, Descripcion, Cantidad FROM CatProducto ORDER BY ID");
        lblTotalR.Text = $"Total de registros: {dgvProducto.Rows.Count}";
    }

    private void AgregarProducto()
    {
        if (txtDesc.Text.Trim() == "" || !int.TryParse(txtCant.Text, out int cant) || cant < 0)
        { Aviso("Escribe una descripción y una cantidad válida."); return; }
        db.Ejecutar("INSERT INTO CatProducto (Descripcion, Cantidad) VALUES (@desc, @cant)",
            ("@desc", txtDesc.Text.Trim()), ("@cant", cant));
        CargarProducto();
        Info("Producto y cantidad registrados correctamente.", "Registro exitoso");
    }

    private void EditarProducto()
    {
        var id = IdSeleccionado(dgvProducto);
        if (id == null) { Aviso("Selecciona un producto."); return; }
        if (!int.TryParse(txtCant.Text, out int cant) || cant < 0) { Aviso("Cantidad inválida."); return; }
        db.Ejecutar("UPDATE CatProducto SET Descripcion = @desc, Cantidad = @cant WHERE ID = @id",
            ("@desc", txtDesc.Text.Trim()), ("@cant", cant), ("@id", id.Value));
        CargarProducto();
        Info("El producto se actualizó correctamente.", "Actualización exitosa");
    }

    private void EliminarProducto()
    {
        var id = IdSeleccionado(dgvProducto);
        if (id == null) { Aviso("Selecciona un producto."); return; }
        if (!Confirmar("¿Deseas eliminar el producto seleccionado?")) return;
        db.Ejecutar("DELETE FROM CatProducto WHERE ID = @id", ("@id", id.Value));
        CargarProducto();
    }
}
