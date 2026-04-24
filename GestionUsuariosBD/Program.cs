/*
 * Created by SharpDevelop.
 * User: jdcad
 * Date: 24/4/2026
 * Time: 1:47 a. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;

namespace GestionUsuariosBD
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			// Inicializar la base de datos (crear tabla si no existe)
            DatabaseHelper.InicializarBaseDatos();
            
			Application.Run(new MainForm());
		}
		
	}
}
