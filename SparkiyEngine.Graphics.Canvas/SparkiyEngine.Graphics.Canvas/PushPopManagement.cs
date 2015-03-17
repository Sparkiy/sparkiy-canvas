using System.Collections.Generic;

namespace SparkiyEngine.Graphics.Canvas
{
	internal class PushPopManagement<T>
	{
		private readonly Stack<T> stack = new Stack<T>();
		private readonly Dictionary<string, T> map = new Dictionary<string, T>();


		/// <summary>
		/// Pushes the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Push(T item)
		{
			this.stack.Push(item);
		}

		/// <summary>
		/// Pops this instance.
		/// </summary>
		/// <returns></returns>
		public T Pop()
		{
			if (this.stack.Count == 0)
				return default(T);
			return this.stack.Pop();
		}

		/// <summary>
		/// Saves the specified item.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="item">The item.</param>
		public void Save(string key, T item)
		{
			this.map[key] = item;
			this.Push(item);
		}

		/// <summary>
		/// Loads the specified item.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Returns saved item.</returns>
		public T Load(string key)
		{
			if (!this.map.ContainsKey(key))
				return default(T);
			return this.map[key];
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			this.stack.Clear();
			this.map.Clear();
		}
	}
}