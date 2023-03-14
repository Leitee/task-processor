namespace TaskProcessor.Core;

public interface IIncludable { }

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IIncludable<out TEntity> : IIncludable { }

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TProperty"></typeparam>
public interface IIncludable<out TEntity, out TProperty> : IIncludable<TEntity> { }