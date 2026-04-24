/*
 * Created by SharpDevelop.
 * User: jdcad
 * Date: 24/4/2026
 * Time: 1:47 a. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GestionUsuariosBD
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			CargarUsuarios(); // Al abrir el formulario, muestra la lista
		}
		
		// Método para cargar todos los usuarios en el DataGridView
        private void CargarUsuarios()
        {
            string sql = "SELECT Id, Nombre, Rol FROM Usuarios ORDER BY Nombre";
            DataTable dt = DatabaseHelper.EjecutarConsulta(sql);
            dgvUsuarios.DataSource = dt;
        }

        // Botón Agregar: abre el formulario de ingreso
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            using (var formUsuario = new FormUsuario())
            {
                if (formUsuario.ShowDialog() == DialogResult.OK)
                {
                    CargarUsuarios(); // refrescar la lista
                }
            }
        }

        // Botón Editar: permite modificar el usuario seleccionado
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["Id"].Value);
            string nombre = dgvUsuarios.CurrentRow.Cells["Nombre"].Value.ToString();

            using (var formUsuario = new FormUsuario(id))
            {
                if (formUsuario.ShowDialog() == DialogResult.OK)
                {
                    CargarUsuarios();
                }
            }
        }

        // Botón Eliminar: borra el usuario seleccionado
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["Id"].Value);
            string nombre = dgvUsuarios.CurrentRow.Cells["Nombre"].Value.ToString();

            DialogResult dr = MessageBox.Show($"¿Está seguro de eliminar a '{nombre}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string sql = "DELETE FROM Usuarios WHERE Id = @id";
                var param = new System.Collections.Generic.Dictionary<string, object> { { "@id", id } };
                DatabaseHelper.EjecutarComando(sql, param);
                CargarUsuarios();
            }
	}
}
