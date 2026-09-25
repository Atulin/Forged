using Forged.Core.Generators;

namespace Forged.Core;

public sealed class GenerationScope : IDisposable
{
	private readonly Dictionary<string, object> _slots = [];
	private bool _disposed;

	internal GenerationScope(Forge forge, GenerationScope? previous)
	{
		Forge = forge;
		Previous = previous;
	}

	internal Forge Forge { get; }

	internal GenerationScope? Previous { get; }

	public void Register<T>(string key, IGenerator<T>? generator)
	{
		ThrowIfDisposed();
		ArgumentException.ThrowIfNullOrEmpty(key);
		_slots[key] = new Slot<T>(generator);
	}

	public T Get<T>(string key)
	{
		var slot = GetSlot<T>(key);
		if (!slot.HasGenerator)
		{
			throw new InvalidOperationException($"No generator is registered for '{key}'.");
		}

		return slot.Get();
	}

	public T GetOrDefault<T>(string key)
		=> GetSlot<T>(key).GetOrDefault();

	internal void End()
	{
		if (_disposed)
		{
			return;
		}

		_disposed = true;
		Forge.EndGeneration(this);
		_slots.Clear();
	}

	public void Dispose()
		=> End();

	private Slot<T> GetSlot<T>(string key)
	{
		ThrowIfDisposed();
		ArgumentException.ThrowIfNullOrEmpty(key);

		if (!_slots.TryGetValue(key, out var slot))
		{
			throw new InvalidOperationException($"Property '{key}' is not registered in the current generation scope.");
		}

		if (slot is not Slot<T> typedSlot)
		{
			throw new InvalidOperationException($"Property '{key}' was registered with a different value type.");
		}

		return typedSlot;
	}

	private void ThrowIfDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException(nameof(GenerationScope));
		}
	}

	private sealed class Slot<T>(IGenerator<T>? generator)
	{
		private T? _value;
		private bool _hasValue;

		public bool HasGenerator => generator is not null;

		public T Get()
		{
			if (!HasGenerator)
			{
				throw new InvalidOperationException("The property has no configured generator.");
			}

			if (!_hasValue)
			{
				_value = generator!.Generate();
				_hasValue = true;
			}

			return _value!;
		}

		public T GetOrDefault()
			=> HasGenerator ? Get() : default!;
	}
}
