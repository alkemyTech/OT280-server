using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OngProject.Core.Models;

namespace OngProject.DataAccess
{
    public static class AppDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<Users> userManager,
            RoleManager<Roles> roleManager)
        {
            #region Create Roles

            //Create Roles
            var adminRole = new Roles{ Name = "admin"};
            var standardRole = new Roles { Name = "standard" };

            if (await roleManager.Roles.AllAsync(r => r.Name != adminRole.Name))
                await roleManager.CreateAsync(adminRole);
            
            if (await roleManager.Roles.AllAsync(r => r.Name != standardRole.Name))
                await roleManager.CreateAsync(standardRole);

            #endregion

            #region Create Users

            //Create Users
            for (var i = 1; i <= 10; i++)
            {
                var admin = new Users
                {
                    FirstName = "FirstName " + i,
                    LastName = "LastName " + i,
                    Email = "Admin" + i + "@example.com",
                    UserName = "admin"+i,
                    Photo = null,
                };

                if (userManager.Users.All(u => u.UserName != admin.UserName))
                {
                    await userManager.CreateAsync(admin,"Abc123.");
                    await userManager.AddToRoleAsync(admin, adminRole.Name);
                }
            }
            
            for (var i = 1; i <= 10; i++)
            {
                var standard = new Users
                {
                    FirstName = "FirstName " + i,
                    LastName = "LastName " + i,
                    Email = "Standard" + i + "@example.com",
                    UserName = "standard"+i,
                    Photo = null,
                };

                if (userManager.Users.All(u => u.UserName != standard.UserName))
                {
                    await userManager.CreateAsync(standard,"Abc123.");
                    await userManager.AddToRoleAsync(standard, standardRole.Name);
                }
            }

            #endregion
            
        }
        public static async Task SeedMembers(AppDbContext context)
        {
            if (!context.Members.Any())
            {
                context.Members.Add(new Members
                {
                    Name = "Juan",
                    FacebookUrl = "juan@facebook",
                    InstagramUrl = "juan@instagram",
                    LinkedinUrl = "juan@linkedin",
                    Description = "Juan es un miembro",
                    Image = ""
                });
                
                context.Members.Add(new Members
                {
                    Name = "Gonzalo",
                    FacebookUrl = "Gonzalo@facebook",
                    InstagramUrl = "Gonzalo@instagram",
                    LinkedinUrl = "Gonzalo@linkedin",
                    Description = "Gonzalo es un miembro",
                    Image = ""
                });
            }
            await context.SaveChangesAsync();
        }

        public static async Task SeedCategories(AppDbContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.Add(new Categories
                {
                    Name = "Educación",
                    Description = "Educación",
                    Image = "",
                    //byte[] ChangeCheck 
                    //bool IsDeleted 
                    //ICollection <News> News { get; set; }
                    //consultar si agrego las novedades
                });
                context.Categories.Add(new Categories
                {
                    Name = "Deportes",
                    Description = "Deportes",
                    Image = "",
                });
                context.Categories.Add(new Categories
                {
                    Name = "Primera infancia",
                    Description = "Primera infancia",
                    Image = "",
                });
                context.Categories.Add(new Categories
                {
                    Name = "Salud",
                    Description = "Salud",
                    Image = "",
                });
                context.Categories.Add(new Categories
                {
                    Name = "Alimentación",
                    Description = "Alimentación",
                    Image = "",
                });
                context.Categories.Add(new Categories
                {
                    Name = "Trabajo Social",
                    Description = "Trabajo Social",
                    Image = "",
                });
            }
            await context.SaveChangesAsync();
        }

        public static async Task SeedActivities(AppDbContext context)
        {
            if (!context.Activities.Any())
            {
                context.Activities.Add(new Activities
                {
                    Name = "Programas Educativos",
                    Content = "Mediante nuestros programas educativos, buscamos incrementar la capacidad"
                                + "intelectual, moral y afectiva de las personas de acuerdo con la cultura y las"
                                    + "normas de convivencia de la sociedad a la que pertenecen.",
                    Image = "image.png"
                });
                context.Activities.Add(new Activities
                {
                    Name = "Apoyo Escolar para el nivel Primario",
                    Content = "El espacio de apoyo escolar es el corazón del área educativa. Se realizan los"
                    + "talleres de lunes a jueves de 10 a 12 horas y de 14 a 16 horas en el contraturno,"
                    + "Los sábados también se realiza el taller para niños y niñas que asisten a la escuela doble turno."
                    + "Tenemos un espacio especial para los de 1er grado 2 veces por semana ya que ellos necesitan atención especial!"
                    + "Actualmente se encuentran inscriptos a este programa 150 niños y niñas de 6 a 15 años.Este taller está"
                    + "pensado para ayudar a los alumnos con el material que traen de la escuela,"
                    + "también tenemos una docente que les da clases de lengua y matemática con una"
                    + "planificación propia que armamos en Manos para nivelar a los niños y que vayan"
                    + "con más herramientas a la escuela.",
                    Image = "image.png"
                });
                context.Activities.Add(new Activities
                {
                    Name = "Apoyo Escolar Nivel Secundaria",
                    Content = "Del mismo modo que en primaria, este taller es el corazón del área secundaria. Se" 
                            + "realizan talleres de lunes a viernes de 10 a 12 horas y de 16 a 18 horas en el"
                            + "contraturno.Actualmente se encuentran inscriptos en el taller 50 adolescentes"
                            + "entre 13 y 20 años.Aquí los jóvenes se presentan con el material que traen del"
                            + "colegio y una docente de la institución y un grupo de voluntarios los recibe para"
                            + "ayudarlos a estudiar o hacer la tarea.Este espacio también es utilizado por los"
                            + "jóvenes como un punto de encuentro y relación entre ellos y la institución.",
                    Image = "image.png"
                });
                context.Activities.Add(new Activities
                {
                    Name = "Tutorias",
                    Content = "Es un programa destinado a jóvenes a partir del tercer año de secundaria, cuyo"
                                + "objetivo es garantizar su permanencia en la escuela y construir un proyecto de"
                                + "vida que da sentido al colegio.El objetivo de esta propuesta es lograr la" 
                                + "integración escolar de niños y jóvenes del barrio promoviendo el soporte" 
                                + "socioeducativo y emocional apropiado, desarrollando los recursos institucionales" 
                                + "necesarios a través de la articulación de nuestras intervenciones con las escuelas que los alojan," 
                                + "con las familias de los alumnos y con las instancias municipales," 
                                + "provinciales y nacionales que correspondan.",
                    Image = "image.png"
                });
                context.Activities.Add(new Activities
                {
                    Name = "Taller Arte y Cuentos",
                    Content = "Taller literario y de manualidades que se realiza semanalmente",
                    Image = "image.png"
                });
                context.Activities.Add(new Activities
                {
                    Name = "Paseos recreativos y educativos",
                    Content = "Estos paseos están pensados para promover la participación y sentido de"
                                + "pertenencia de los niños, niñas y adolescentes al área educativa.",
                    Image = "image.png"
                });
            }
            
            await context.SaveChangesAsync();
        }

        public static async Task SeedTestimonial(AppDbContext context)
        {
            if (!context.Testimonials.Any())
            {
                context.Testimonials.Add(new Testimonials
                {
                    Name = "Juan Cruz",
                    Image = "Imagen1.png",
                    Content = "Es una Ong muy recomendable",       
                });
                context.Testimonials.Add(new Testimonials
                {
                    Name = "Emma",
                    Image = "Imagen2.png",
                    Content = "Tuve una muy buena experiencia",
                });
                context.Testimonials.Add(new Testimonials
                {
                    Name = "Florencia",
                    Image = "Imagen3.png",
                    Content = "Me parece que este es una de esos proyectos que deben " +
                              "recorrer el mundo, con un impacto tan real y tan grande que " +
                              "deben ser un ejemplo para todos",
                });
            }
            await context.SaveChangesAsync();
        }

        public static async Task SeedNews(AppDbContext context)
        {
            if (!context.News.Any())
            {
                context.News.Add(new News
                {
                    Name = "Nuevo Taller Arte y Cuentos para los niños",
                    Content = "Es un taller literario y de manualidades que se realiza semanalmente.",
                    Image = "image.png",
                    CategoryId = 2
                });

                context.News.Add(new News
                {
                    Name = "Apoyo Escolar Nivel Secundaria tambien sábados",
                    Content = "Del mismo modo que se realiza de lunes a viernes, este taller se realizará tambien los días sabádos de 10 a 12 horas para todas las edades.",
                    Image = "image.png",
                    CategoryId = 2
                });
            }
            await context.SaveChangesAsync();
        }
    }
}