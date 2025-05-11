import pandas as pd
import matplotlib.pyplot as plt
import os

# Ruta al CSV (ajústala si es necesario)
csv_path = os.path.join("..", "Results", "car_positions.csv")

# Leer CSV usando separador ';' y punto como decimal
df = pd.read_csv(csv_path, sep=';', decimal=',')

# Eliminar espacios adicionales en los nombres de las columnas
df.columns = df.columns.str.strip()

# Verificar que la columna 'Collected' existe
if 'Collected' not in df.columns:
    raise ValueError("La columna 'Collected' no se encuentra en el CSV.")

# Asegurar que los tipos son correctos
df['Collected'] = df['Collected'].astype(bool)

# Convertir 'Time' a tipo numérico, forzando errores a NaN para manejarlos
df['Time'] = pd.to_numeric(df['Time'], errors='coerce')

# Eliminar filas donde 'Time' sea NaN (si es necesario)
df = df.dropna(subset=['Time'])

# Filtrar las filas donde 'Collected' es True
collected = df[df['Collected']]

# Crear la figura para la gráfica y la tabla
fig, ax = plt.subplots(figsize=(10, 6))

# Dibujar trayectoria
ax.plot(df['X'], df['Z'], label='Trayectoria', color='blue')

# Marcar los objetos recogidos con orden
for i, row in collected.reset_index().iterrows():
    ax.scatter(row['X'], row['Z'], color='red', zorder=5)
    ax.text(row['X'], row['Z'], str(i + 1), fontsize=9, color='black', ha='center', va='bottom')

# Crear la tabla con los objetos recogidos y los tiempos
collected_objects = collected[['Time', 'X', 'Y', 'Z']]
collected_objects['Object'] = collected.index + 1  # Asigna el índice + 1 como el número de objeto

# Extraer los tiempos de recogida
times = [f"{time:.2f}s" for time in collected_objects['Time'].values]

# Crear los nombres de las columnas como "Object 1", "Object 2", etc.
columns = ['Objects'] + ['Object ' + str(i + 1) for i in range(len(times))]

# Agregar la tabla debajo de la gráfica
table_data = [['Time (s)'] + list(times)]  # Datos de la tabla con encabezado "Time (s)"
ax.table(cellText=table_data, colLabels=columns, cellLoc='center', loc='bottom', bbox=[0.1, -0.3, 0.8, 0.15])

# Ajustar el tamaño para que la tabla encaje correctamente
plt.subplots_adjust(bottom=0.21)

# Título y etiquetas de la gráfica
ax.set_title('Trayectoria del coche (vista superior)')
ax.set_xlabel('Posición X')
ax.set_ylabel('Posición Z')
ax.legend()
ax.grid(True)

# Guardar la imagen
output_path = os.path.join("..", "Results", "trayectoria_2d_con_tabla.png")
plt.savefig(output_path)
plt.show()
