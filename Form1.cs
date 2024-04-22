using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace PlayMp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        private void btBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "File MP3|*.mp3";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = openFileDialog.FileName;
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string path = txtPath.Text;
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(path))
            {
                ListViewItem item = new ListViewItem(listMusic.Items.Count.ToString());
                item.SubItems.Add(name);
                item.SubItems.Add(path);
                listMusic.Items.Add(item);

                // Xóa dữ liệu đã nhập sau khi thêm vào ListView
                txtName.Clear();
                txtPath.Clear();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên và đường dẫn của bài nhạc!");
            }
        }

        private void btPlay_Click(object sender, EventArgs e)
        {
            if (listMusic.SelectedItems.Count > 0)
            {
                string selectedPath = listMusic.SelectedItems[0].SubItems[2].Text; // Lấy đường dẫn từ dòng được chọn

                if (outputDevice != null)
                {
                    outputDevice.Stop(); // Dừng nếu đang phát nhạc khác
                }

                outputDevice = new WaveOutEvent();
                audioFile = new AudioFileReader(selectedPath);
                outputDevice.Init(audioFile);
                outputDevice.Play();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một bài hát để phát.");
            }
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            if (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                outputDevice.Stop();
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

                // Di chuyển đến vị trí trong nhạc tương ứng với giá trị của scrollbar
                float percentage = (float)hScrollBar1.Value / hScrollBar1.Maximum;
                audioFile.CurrentTime = TimeSpan.FromSeconds(percentage * audioFile.TotalTime.TotalSeconds);

        }
    }
}
