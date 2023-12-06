import time
import board
import adafruit_dht
import sqlite3
import datetime
import logging
import sys

logging.basicConfig(filename='python.log', encoding='utf-8', level=logging.INFO)
logging.getLogger().addHandler(logging.StreamHandler(sys.stdout))

read_interval_seconds = 10
gpio_port = board.D19
db_name = "weather_stats.db"

def main():  
  try:
    while True:
      try:
        temperature, humidity = get_stats()
      except RuntimeError as error:
        time.sleep(5)
        continue
        
      insert_data(temperature, humidity)
      
      time.sleep(read_interval_seconds)         
        
  except Exception as error:    
    logging.error(error.args[0])

def get_stats():
  
  try:        
    # you can pass DHT22 use_pulseio=False if you wouldn't like to use pulseio.
    # This may be necessary on a Linux single board computer like the Raspberry Pi,
    # but it will not work in CircuitPython.
    dhtDevice = adafruit_dht.DHT22(gpio_port, use_pulseio=False)    
    temperature = dhtDevice.temperature
    humidity = dhtDevice.humidity    
 
    return temperature, humidity
    
  except RuntimeError as error:
    # Errors happen fairly often, DHT's are hard to read, just keep going
    logging.warning(error.args[0])
    raise error
    
  except Exception as error:
    dhtDevice.exit()
    raise error
    
    
def insert_data(temperature, humidity):
  
  connection = sqlite3.connect(db_name)
  
  try:
    
    cursor = connection.cursor()
  
    time_in_seconds = round(time.time())
    now = datetime.datetime.now().strftime("%d-%m-%Y %H:%M")    

    cursor.execute("INSERT INTO weather_stats(time, temperature, humidity) VALUES (?, ?, ?)", (time_in_seconds, temperature, humidity))    
    connection.commit()
        
    logging.info("\n{}: \tTemperature: {:.1f} °C \tHumidity: {}% \tTime: {}s".format(now, temperature, humidity, time_in_seconds))
    print("\n{}: \tTemperature: {:.1f} °C \tHumidity: {}% \tTime: {}s".format(now, temperature, humidity, time_in_seconds))
    
    connection.close()
  
  except Exception as error:
    connection.close()
    raise error

if __name__ == '__main__':
    main()