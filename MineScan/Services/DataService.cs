using System;
using System.IO;
using System.Text.Json;
using MineScan.Models;

namespace MineScan.Services;

public class DataService
{
    public static DataService Instance { get; } = new();
    private readonly string _filePath;
    public AppData LocalData { get; private set; }
    
    private DataService()
    {
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        LocalData = Load();
    }
    
    private AppData Load()
    {
        if (!File.Exists(_filePath))
        {
            return new AppData();
        }

        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<AppData>(json) ?? new AppData();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Помилка читання конфігу: {ex.Message}");
            return new AppData();
        }
    }
    
    public void Save()
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(LocalData, options);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Помилка збереження конфігу: {ex.Message}");
        }
    }
}