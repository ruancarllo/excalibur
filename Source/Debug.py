import os
import signal
import shutil
import subprocess

EXCALIBUR_ASSEMBLY_PATH = './Context/bin/net7.0/Excalibur.rhp'

RHINOCEROS_PLUGIN_DIRECTORY = "/Applications/Rhino 7.app/Contents/PlugIns"
RHINOCEROS_EXECUTABLE_PATH = "/Applications/Rhino 7.app/Contents/MacOS/Rhinoceros"

def compile_dotnet_project():
  subprocess.run(['dotnet', 'build'])

def copy_or_rewrite_file(file_path, destination_directory_path):
  file_name = os.path.basename(file_path)
  destination_path = os.path.join(destination_directory_path, file_name)

  shutil.copy2(file_path, destination_path)

def open_rhino_process(rhinoceros_executable_path):
  rhino_process = subprocess.Popen([rhinoceros_executable_path, '-nosplash', '-new'])
  rhino_process_id = os.getpgid(rhino_process.pid)

  try: rhino_process.wait()
  except KeyboardInterrupt: os.killpg(rhino_process_id, signal.SIGINT)  


compile_dotnet_project()
copy_or_rewrite_file(EXCALIBUR_ASSEMBLY_PATH, RHINOCEROS_PLUGIN_DIRECTORY)
open_rhino_process(RHINOCEROS_EXECUTABLE_PATH)