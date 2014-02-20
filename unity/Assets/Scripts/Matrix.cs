using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Matrix
{
	private List<List<float>> m_matrix; 

	public int RowCount
	{
		get { return m_matrix.Count; }
	}

	public int ColumnCount
	{
		get { return m_matrix [0].Count; }
	}

	public Matrix(int rows, int columns)
	{
		m_matrix = new List<List<float>> (rows);

		for(int row = 0; row < rows; rows++ 
		{
			m_matrix[row] = new List<float>(columns);
		}
	}

	/// <summary>
	/// Builds an matrix using an array and the column lengths to figure out how many rows a necessary.
	/// </summary>
	/// <param name="array">Array.</param>
	/// <param name="columns">Columns.</param>
	public Matrix( float[] array, int columns)
	{
		m_matrix = new List<List<float>> (array.Length / columns);

		foreach (List<float> row in m_matrix) {
			row = new List<float>(columns);
		}
	}

	/// <summary>
	/// Can be used for setting values in the matrix.	/// </summary>
	/// <param name="row">Row.</param>
	public List<float> GetRow(int row)
	{
		return m_matrix[row];
	}

	/// <summary>
	/// Should not be used for setting values in the matrix.	/// </summary>
	/// <param name="column">Column.</param>
	public List<float> GetColumn(int column)
	{
		List<float> columnVector = new List<float>();

		foreach (List<float> row in m_matrix) {
			columnVector.Add (row[column]);
		}

		return columnVector;
	}

	/// <summary>
	/// Gets a value at the specified row and column	/// </summary>
	/// <returns>The value.</returns>
	/// <param name="row">Row.</param>
	/// <param name="column">Column.</param>
	public float GetValue(int row, int column)
	{
		return m_matrix [row] [column];
	}

	public void SetValue(int row, int column, float value)
	{
		m_matrix [row] [column] = value;
	}

	public static Matrix operator+(Matrix a, Matrix b)
	{
		Matrix result = null;

		if (a.RowCount != b.RowCount || a.ColumnCount != b.ColumnCount) {
			result = new Matrix (a.RowCount, a.ColumnCount);

			for (int row = 0; row < a.RowCount; row++)
			{
				for (int column = 0; column < a.ColumnCount; column++)
				{
					result.SetValue(row, column, a.GetValue(row,column)+ b.GetValue(row,column));
				}
			}
		}
		return result;
	}
}


