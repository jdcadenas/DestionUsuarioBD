/*
 * Created by SharpDevelop.
 * User: jdcad
 * Date: 24/4/2026
 * Time: 2:22 a. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace GestionUsuariosBD
{
	/// <summary>
    /// Clase estática que maneja todas las operaciones con la base de datos SQLite.
    /// Proporciona métodos para inicializar, ejecutar comandos y consultas.
    /// </summary>
    public static class DatabaseHelper
    {
        // Cadena de conexión: apunta al archivo 'usuarios.db' que se crea en la misma carpeta del ejecutable
        private static string connectionString = "Data Source=usuarios.db;Version=3;";

        /// <summary>
        /// Crea la tabla 'Usuarios' si no existe.
        /// Debe llamarse al inicio del programa.
        /// </summary>
        public static void InicializarBaseDatos()
        {
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"
                        CREATE TABLE IF NOT EXISTS Usuarios (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Nombre TEXT NOT NULL,
                            Clave TEXT NOT NULL,
                            Rol TEXT NOT NULL
                        )";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Ejecuta un comando SQL que no devuelve datos (INSERT, UPDATE, DELETE).
        /// Permite pasar parámetros para evitar inyección SQL.
        /// </summary>
        /// <param name="sql">Texto del comando SQL (puede contener parámetros con @nombre)</param>
        /// <param name="parametros">Diccionario con los nombres y valores de los parámetros</param>
        public static void EjecutarComando(string sql, Dictionary<string, object> parametros = null)
        {
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        if (parametros != null)
                        {
                            foreach (var p in parametros)
                                cmd.Parameters.AddWithValue(p.Key, p.Value);
                        }
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar comando: " + ex.Message, "Error BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Ejecuta una consulta SELECT y devuelve los resultados en un DataTable.
        /// Útil para llenar DataGridView o ComboBox.
        /// </summary>
        /// <param name="sql">Consulta SQL (puede contener parámetros)</param>
        /// <param name="parametros">Parámetros opcionales</param>
        /// <returns>DataTable con los datos obtenidos</returns>
        public static DataTable EjecutarConsulta(string sql, Dictionary<string, object> parametros = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        if (parametros != null)
                        {
                            foreach (var p in parametros)
                                cmd.Parameters.AddWithValue(p.Key, p.Value);
                        }
                        using (var da = new SQLiteDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en consulta: " + ex.Message, "Error BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }
    }
}
