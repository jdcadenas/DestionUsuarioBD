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
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace GestionUsuariosBD
{
	/// <summary>
	/// Formulario Principal para la gestión de usuarios.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			CargarUsuarios();
		}
		
		private void CargarUsuarios(string filtro = "")
		{
			string sql = "SELECT Id, Nombre, Rol FROM Usuarios";
			Dictionary<string, object> param = null;

			if (!string.IsNullOrEmpty(filtro))
			{
				sql += " WHERE Nombre LIKE @filtro";
				param = new Dictionary<string, object> { { "@filtro", "%" + filtro + "%" } };
			}
			
			sql += " ORDER BY Nombre";
			DataTable dt = DatabaseHelper.EjecutarConsulta(sql, param);
			dgvUsuarios.DataSource = dt;
		}

		private void btnBuscar_Click(object sender, EventArgs e)
		{
			CargarUsuarios(txtBuscar.Text);
		}

		private void btnAgregar_Click(object sender, EventArgs e)
		{
			using (var formUsuario = new FormUsuario())
			{
				if (formUsuario.ShowDialog() == DialogResult.OK)
				{
					CargarUsuarios();
				}
			}
		}

		private void btnEditar_Click(object sender, EventArgs e)
		{
			if (dgvUsuarios.CurrentRow == null)
			{
				MessageBox.Show("Seleccione un usuario para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			int id = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["Id"].Value);

			using (var formUsuario = new FormUsuario(id))
			{
				if (formUsuario.ShowDialog() == DialogResult.OK)
				{
					CargarUsuarios();
				}
			}
		}

		private void btnEliminar_Click(object sender, EventArgs e)
		{
			if (dgvUsuarios.CurrentRow == null)
			{
				MessageBox.Show("Seleccione un usuario para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			int id = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["Id"].Value);
			string nombre = dgvUsuarios.CurrentRow.Cells["Nombre"].Value.ToString();

			DialogResult dr = MessageBox.Show(string.Format("¿Está seguro de eliminar a '{0}'?", nombre), "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dr == DialogResult.Yes)
			{
				string sql = "DELETE FROM Usuarios WHERE Id = @id";
				var param = new Dictionary<string, object> { { "@id", id } };
				DatabaseHelper.EjecutarComando(sql, param);
				CargarUsuarios();
			}
		}
	}
}
