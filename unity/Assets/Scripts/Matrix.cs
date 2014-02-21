using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Matrix
{
	public List<List<float>> m_matrix; 

	#region Accessors
	public int RowCount
	{
		get { return m_matrix.Count; }
	}

	public int ColumnCount
	{
		get { return m_matrix [0].Count; }
	}
	#endregion

	#region Constructors
	public Matrix(int rows, int columns)
	{
		m_matrix = new List<List<float>>();

		for(int row = 0; row < rows; row++)
		{
			m_matrix.Add(new List<float>());

			for (int column = 0; column < columns; column++)
			{
				m_matrix[row].Add (0);
			}
		}
	}

	/// <summary>
	/// Builds an matrix using an array and the column lengths to figure out how many rows a necessary.
	/// </summary>
	/// <param name="array">Array.</param>
	/// <param name="columns">Columns.</param>
	public Matrix( float[] array, int columns)
	{
		int rows = array.Length/columns;
		m_matrix = new List<List<float>>();

		int arrayLocation = 0;
		for(int row = 0; row < rows; row++)
		{
			m_matrix.Add (new List<float>());
			for (int setcolumn = 0; setcolumn < columns; setcolumn++)
			{
				m_matrix[row].Add(array[arrayLocation]);
				arrayLocation++;
			}
		}
	}

	public Matrix(Matrix oldMatrix)
	{
		m_matrix = new List<List<float>>();

		for (int row = 0; row < oldMatrix.RowCount; row++)
		{
			m_matrix.Add (new List<float>());

			for (int column = 0; column < oldMatrix.ColumnCount; column++)
			{
				m_matrix[row].Add (oldMatrix[row][column]);
			}
		}
	}
	#endregion

	/// <summary>
	/// Assumes that the matrix object is already created a sized correctly.
	/// Sets the values of a matrix. float.NaN values will be ignored	/// </summary>
	/// <param name="array">Array.</param>
	/// <param name="columns">Columns.</param>
	public void SetMatrix( float[] array, int columns)
	{
		int rows = array.Length/columns;
		
		int arrayLocation = 0;
		for(int row = 0; row < rows; row++)
		{			
			for (int setcolumn = 0; setcolumn < columns; setcolumn++)
			{
				if (!float.IsNaN(array[arrayLocation]))
				{
					m_matrix[row][setcolumn] = array[arrayLocation];
				}
				arrayLocation++;
			}
		}
	}

	public void SetMatrix( Matrix a)
	{
		for (int row = 0; row < a.RowCount; row++)
		{
			for(int column = 0; column < a.ColumnCount; column++)
			{
				m_matrix[row][column] = a[row][column];
			}
		}
	}

	private string m_printList(List<float> list)
	{
		string matrix = string.Empty;
		foreach (float entry in list)
		{
			matrix += string.Format ("{0} ", entry);
		}

		return matrix;
	}

	public override string ToString ()
	{
		string matrixString = string.Empty;

		foreach (List<float> row in m_matrix)
		{
			matrixString += "\n" + m_printList (row);
		}

		return matrixString;
	}

	/// <summary>
	/// Gets the specified row by value	/// </summary>
	/// <returns>The row.</returns>
	/// <param name="row">Row.</param>
	public List<float> GetRow(int row)
	{
		return new List<float>(m_matrix[row]);
	}

	/// <summary>
	/// Gets the specified column by value	/// </summary>
	/// <returns>The column.</returns>
	/// <param name="column">Column.</param>
	public List<float> GetColumn(int column)
	{
		List<float> columnVector = new List<float>();

		foreach (List<float> row in m_matrix) {
			columnVector.Add (row[column]);
		}

		return columnVector;
	}

	public void Print(string name)
	{
		Debug.Log(name+"\n"+ this.ToString());
	}

	/// <summary>
	/// Gets the specified row by reference.	/// </summary>
	/// <param name="row">Row.</param>
	public List<float> this[int row]
	{
			get{ return m_matrix[row]; }
	}

	#region Overloaded Operators
	public static Matrix operator+(Matrix a, Matrix b)
	{
		Matrix result = new Matrix (a.RowCount, a.ColumnCount);

		for (int row = 0; row < a.RowCount; row++)
		{
			for (int column = 0; column < a.ColumnCount; column++)
			{
				result[row][column] = a[row][column] + b[row][column];
			}
		}

		return result;
	}

	public static Matrix operator+(float a, Matrix b)
	{
		Matrix result = new Matrix(b);

		for (int row = 0; row < b.RowCount; row++)
		{
			for (int column = 0; column < b.ColumnCount; column++)
			{
				result[row][column] += a;
			}
		}
		
		return result;
	}

	public static Matrix operator+(Matrix b, float a)
	{
		return a+b;
	}

	public static Matrix operator-(Matrix a, Matrix b)
	{
		Matrix result = new Matrix (a.RowCount, a.ColumnCount);
		
		for (int row = 0; row < a.RowCount; row++)
		{
			for (int column = 0; column < a.ColumnCount; column++)
			{
				result[row][column] = a[row][column] - b[row][column];
			}
		}
		
		return result;
	}

	public static Matrix operator-(float a, Matrix b)
	{
		return (-1*a)+b;
	}

	public static Matrix operator-(Matrix b, float a)
	{
		return (-1*a)+b;
	}

	public static Matrix operator*(Matrix a, Matrix b)
	{
		Matrix result = new Matrix (a.RowCount, b.ColumnCount);
		
		for (int row = 0; row < a.RowCount; row++)
		{
			for (int column = 0; column < b.ColumnCount; column++)
			{
				float sum = 0;

				for (int multiplier = 0; multiplier < a.RowCount; multiplier++)
				{
					sum += a[row][multiplier] * b[multiplier][column];
				}
				result[row][column] = sum;
			}
		}
		
		return result;
	}

	public static Matrix operator*(float a, Matrix b)
	{
		Matrix result = new Matrix (b);
		
		for (int row = 0; row < b.RowCount; row++)
		{
			for (int column = 0; column < b.ColumnCount; column++)
			{
				result[row][column] *= a;
			}
		}
		
		return result;
	}

	public static Matrix operator*(Matrix b, float a)
	{
		return a*b;
	}

	public static Matrix operator/(Matrix b, float a)
	{
		return (1/a)*b;
	}
	#endregion


}




