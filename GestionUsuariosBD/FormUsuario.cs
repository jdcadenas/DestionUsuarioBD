/*
 * Created by SharpDevelop.
 * User: jdcad
 * Date: 24/4/2026
 * Time: 2:41 a. m.
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
	/// Formulario para Agregar y Editar Usuarios.
	/// </summary>
	public partial class FormUsuario : Form
	{
		private int? usuarioId = null;

		public FormUsuario()
		{
			InitializeComponent();
			this.Text = "Nuevo Usuario";
		}

		// Constructor para edición
		public FormUsuario(int id)
		{
			InitializeComponent();
			this.usuarioId = id;
			this.Text = "Editar Usuario";
			CargarDatosUsuario();
		}

		private void CargarDatosUsuario()
		{
			if (usuarioId.HasValue)
			{
				string sql = "SELECT * FROM Usuarios WHERE Id = @id";
				var param = new Dictionary<string, object> { { "@id", usuarioId.Value } };
				var dt = DatabaseHelper.EjecutarConsulta(sql, param);

				if (dt.Rows.Count > 0)
				{
					txtNombre.Text = dt.Rows[0]["Nombre"].ToString();
					txtClave.Text = dt.Rows[0]["Clave"].ToString();
					cmbRol.SelectedItem = dt.Rows[0]["Rol"].ToString();
				}
			}
		}

		private void btnGuardar_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtClave.Text) || cmbRol.SelectedItem == null)
			{
				MessageBox.Show("Por favor complete todos los campos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			string sql;
			var param = new Dictionary<string, object>
			{
				{ "@nombre", txtNombre.Text },
				{ "@clave", txtClave.Text },
				{ "@rol", cmbRol.SelectedItem.ToString() }
			};

			if (usuarioId.HasValue)
			{
				sql = "UPDATE Usuarios SET Nombre = @nombre, Clave = @clave, Rol = @rol WHERE Id = @id";
				param.Add("@id", usuarioId.Value);
			}
			else
			{
				sql = "INSERT INTO Usuarios (Nombre, Clave, Rol) VALUES (@nombre, @clave, @rol)";
			}

			DatabaseHelper.EjecutarComando(sql, param);
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnCancelar_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
