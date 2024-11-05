using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using WindowsInput.Native;
using WindowsInput;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FrontTCC.Services
{
    public class WhatsAppService
    {
        // Importa a função para buscar uma janela ativa pelo título
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool IsWindow(IntPtr hWnd);

        // Método para verificar se uma das janelas do WhatsApp está aberta
        private bool IsWhatsAppWindowOpen()
        {
            string[] titles = { "WhatsApp", "WhatsApp Web", "Compartilhe no WhatsApp",
                                "WhatsApp - Google Chrome", "WhatsApp Web - Google Chrome",
                                "Compartilhe no WhatsApp - Google Chrome" };

            foreach (string title in titles)
            {
                IntPtr hWnd = FindWindow(null, title);
                if (IsWindow(hWnd))
                {
                    return true; // Retorna true se qualquer uma das janelas for encontrada
                }
            }
            return false; // Retorna false se nenhuma das janelas estiver aberta
        }

        public async Task OpenWhatsAppChat(string phoneNumber, string message)
        {
            string encodedMessage = System.Net.WebUtility.UrlEncode(message);
            string url = $"https://wa.me/{phoneNumber}?text={encodedMessage}";

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });

            await Task.Delay(5000); // Aguarda o navegador abrir a aba

            var simulator = new InputSimulator();

            // Loop para verificar se uma das janelas do WhatsApp ainda está aberta
            while (IsWhatsAppWindowOpen())
            {
                // Verificar novamente se a janela ainda está aberta antes de cada ação
                if (!IsWhatsAppWindowOpen()) break;
                simulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
                await Task.Delay(500);

                if (!IsWhatsAppWindowOpen()) break;
                simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                await Task.Delay(500);

                if (!IsWhatsAppWindowOpen()) break;
                simulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
                await Task.Delay(500);

                if (!IsWhatsAppWindowOpen()) break;
                simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                await Task.Delay(3000);

                if (!IsWhatsAppWindowOpen()) break;
                simulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
                await Task.Delay(500);

                if (!IsWhatsAppWindowOpen()) break;
                simulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
                await Task.Delay(500);

                if (!IsWhatsAppWindowOpen()) break;
                simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                await Task.Delay(10000);

                if (!IsWhatsAppWindowOpen()) break;
                simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                await Task.Delay(3000);

                // Fecha a aba do navegador se ainda estiver aberta
                if (IsWhatsAppWindowOpen())
                {
                    simulator.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
                    simulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
                    simulator.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
                }

                break; // Interrompe o loop ao concluir as ações
            }
        }
    }
}
