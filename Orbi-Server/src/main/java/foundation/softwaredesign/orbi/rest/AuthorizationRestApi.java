package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.model.auth.AuthorizationInfo;
import foundation.softwaredesign.orbi.model.auth.LoginInfo;
import foundation.softwaredesign.orbi.model.auth.RequestCodeInfo;
import foundation.softwaredesign.orbi.service.auth.UserService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.validation.constraints.NotNull;
import javax.ws.rs.*;
import javax.ws.rs.core.Response;
import java.util.logging.Logger;

import static javax.ws.rs.core.MediaType.APPLICATION_JSON;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Path("/auth")
@Produces({APPLICATION_JSON})
@Consumes({APPLICATION_JSON})
@RequestScoped
public class AuthorizationRestApi {

    @Inject
    UserService userService;

    @GET
    @Path("/user")
    public Response user() {
        // will fail if not authorized - filter
        userService.updateLastInit();
        sleep();
        return Response.status(Response.Status.OK).build();
    }

    @POST
    @Path("/login")
    public AuthorizationInfo login(@NotNull LoginInfo loginInfo) {
        sleep();
        return userService.login(loginInfo);
    }

    @POST
    @Path("/requestcode")
    public Response requestcode(@NotNull RequestCodeInfo requestCodeInfo) {
        sleep();
        userService.requestPassword(requestCodeInfo);
        return Response.status(Response.Status.CREATED).build();
    }

    private void sleep() {
        try {
            Thread.sleep(500);
        } catch (InterruptedException e) {
            Logger.getLogger(UserService.class.getName()).finest("sleeping to prevent login dos");
        }
    }


}
