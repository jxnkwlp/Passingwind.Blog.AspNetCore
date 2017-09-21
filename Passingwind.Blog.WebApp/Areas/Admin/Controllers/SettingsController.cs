using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace Passingwind.Blog.WebApp.Areas.Admin.Controllers
{
    public class SettingsController : AdminControllerBase
    {
        private readonly SettingManager _settingManager;

        public SettingsController(SettingManager settingManager)
        {
            this._settingManager = settingManager;

        }

        public async Task<IActionResult> Basic()
        {
            var settings = await _settingManager.LoadSettingAsync<BasicSettings>();

            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> Basic(BasicSettings model)
        {
            if (ModelState.IsValid)
            {
                await _settingManager.SaveSettingAsync(model);

                AlertSuccess("已保存。");
            }

            return View(model);
        }


        public async Task<IActionResult> Advanced()
        {
            var settings = await _settingManager.LoadSettingAsync<AdvancedSettings>();

            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> Advanced(AdvancedSettings model)
        {
            if (ModelState.IsValid)
            {
                await _settingManager.SaveSettingAsync(model);

                AlertSuccess("已保存。");
            }

            return View(model);
        }


        public async Task<IActionResult> Comments()
        {
            var settings = await _settingManager.LoadSettingAsync<CommentsSettings>();

            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> Comments(CommentsSettings model)
        {
            if (ModelState.IsValid)
            {
                await _settingManager.SaveSettingAsync(model);

                AlertSuccess("已保存。");
            }

            return View(model);
        }

        public async Task<IActionResult> Email()
        {
            var settings = await _settingManager.LoadSettingAsync<EmailSettings>();

            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> Email(EmailSettings model)
        {
            if (ModelState.IsValid)
            {
                await _settingManager.SaveSettingAsync(model);

                AlertSuccess("已保存。");
            }

            return View(model);
        }


    }
}
