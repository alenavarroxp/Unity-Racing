import pandas as pd
import os

# Leer el CSV con una sola columna
csv_path = os.path.join("..", "Results", "car_positions.csv")
df = pd.read_csv(csv_path, header=None)

# Dividir la columna en múltiples
df[['Time', 'X', 'Y', 'Z', 'Collected']] = df[0].str.split(',', expand=True)

# Convertir las columnas a los tipos correctos
df['Time'] = pd.to_numeric(df['Time'], errors='coerce')
df['X'] = pd.to_numeric(df['X'], errors='coerce')
df['Y'] = pd.to_numeric(df['Y'], errors='coerce')
df['Z'] = pd.to_numeric(df['Z'], errors='coerce')
df['Collected'] = df['Collected'].astype(bool)